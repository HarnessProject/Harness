#region ApacheLicense
// From the Harness Project
// Harness.Framework
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;
using PCLStorage;

#endregion

namespace Harness.Framework
{
    public class DomainProvider : IDomainProvider
    {
        protected static Assembly[] _assemblyCache;
        protected static Type[] _typeCache;

        public static DomainProvider Instance { get; } = new DomainProvider();

        public IEnumerable<Assembly> Assemblies { get; } = _assemblyCache = GetAssemblies().ToArray();

        public IEnumerable<Type> Types { get; } = _typeCache = GetTypes().ToArray();

        private DomainProvider()
        {
        }

        #region IDomainProvider Members

        public TY Cast<TY>(object o)
        {
            return this.Try(x => (TY)o).Catch<Exception>((x,ex) => GetDefault<TY>()).Act();
        }

        public T Create<T>(params object[] args)
        {
            var types = Types.Where(x => x.Is<T>());
            var constructors =
                types.Select(
                    x => x.GetConstructor(args.Select(a => a.GetType()).ToArray())
                ).WhereNotDefault();
            return Cast<T>(constructors.FirstOrDefault().Invoke(args));
        }

        public Type CreateGeneric(Type generic, params Type[] genericParameters)
        {
            if (generic.IsGenericTypeDefinition)
                return generic.MakeGenericType(genericParameters);

            throw new InvalidOperationException("Type provided is not an open generic");
        }

        public IFactory<T> FactoryFor<T>()
        {
            return Create<IFactory<T>>();
        }

        public IEnumerable<Type> GetAncestorsOf(Type type)
        {
            while (type.NotNull() &&
                   type.BaseType.NotNull())
            {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }

        public IEnumerable<Type> GetAncestorsOf<T>()
        {
            return GetAncestorsOf(typeof(T));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var files =
                FileSystem.Current
                .GetFolderFromPathAsync(".")
                .AwaitResult()
                .GetFilesAsync()
                .AwaitResult();

            return
                    files
                    .Where(x => x.Name.EndsIn("dll", "exe"))
                    .Select(
                        x =>
                        {
                            var n = x.Name.RemoveExtension();
                            return new AssemblyName
                            {
                                Name = n
                            };
                        }
                    ).Select(x => x.Try(y => Assembly.Load(y.ToString())).Act())
                    .Where(ObjectExtensions.NotNull);
        }

        public object GetDefault(Type t)
        {
            var m = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            return
                m.FirstOrDefault(x => x.Name.Contains("GetDefault") && x.IsGenericMethodDefinition)
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { });
        }

        public T GetDefault<T>()
        {
            return default(T);
        }

        public static IEnumerable<Type> GetTypes(Filter<Type> predicate = null)
        {
            return
               _assemblyCache
                .SelectMany(
                    x => x.Try(
                        y => predicate.NotNull() ?
                            y.GetExportedTypes().Where(predicate.AsFunc()) :
                            y.GetExportedTypes()
                    ).Act()
                ).ToArray();
        }

        public bool IsGeneric(Type target, Type generic)
        {
            if (generic.IsGenericTypeDefinition)
                return
                    GetAncestorsOf(target)
                        .FirstOrDefault(
                            x => x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == generic
                        ).NotNull();
            return false;
        }

        public bool IsGeneric<T>(Type generic)
        {
            if (generic.IsGenericTypeDefinition)
                return
                    GetAncestorsOf<T>()
                        .FirstOrDefault(
                            x => x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == generic
                        ).NotNull();
            return false;
        }

        #endregion

        //This Bug is FIXED!!!
        //private static TY Convert<T, TY>(object source) {
        //    //This fixes a bug in the .Net Framework -
        //    //Enables casting from object to a compatible type in a PCL
        //    //where object has been received as a method parameter
        //    var p = Expression.Parameter(typeof (T));
        //    var converted = Expression.Convert(p, typeof (TY));
        //    var m = Expression.Lambda<Func<T, TY>>(converted, p).Compile();
        //    return m((T) source);
        //}
    }
}