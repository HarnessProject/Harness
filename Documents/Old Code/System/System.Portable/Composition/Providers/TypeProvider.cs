#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright � 2014 Nick Daniels et all, All Rights Reserved.
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

using System.Collections.Generic;
using System.Composition.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Portable;
using System.Portable.Reflection;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace System.Composition.Providers {
    public class TypeProvider : ITypeProvider {
        private static TypeProvider _instance;
        protected Assembly[] AssemblyCache { get; set; }
        protected Type[] TypeCache { get; set; }

        public static TypeProvider Instance {
            get { return _instance ?? (_instance = new TypeProvider()); }
        }

        public TypeProvider() {
           TypeCache = GetTypes().ToArray();
        }

        #region ITypeProvider Members

        public TY Cast<TY>(object o) {
            //This for some UNKNOWN reason is faster than just calling Convert - ?? 
            var m = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            return 
                (TY)m.FirstOrDefault(x => x.Name.Contains("Convert"))
                .MakeGenericMethod(o.GetType(), typeof (TY))
                .Invoke(null, new []{o});
        }

        public IEnumerable<Assembly> Assemblies { get { return AssemblyCache ?? GetAssemblies(); } }
        public IEnumerable<Type> Types { get { return TypeCache ?? GetTypes(); } }

        public object GetDefault(Type t) {
            return
                Provider
                    .Reflector
                    .InvokeGenericMember(this, "GetDefault", new [] {t}, null);
        }

        public T GetDefault<T>() {
            return default(T);
        }

        public IFactory<T> FactoryFor<T>() {
            return Create<IFactory<T>>();
        }

        public T Create<T>(params object[] args) {
            var c = Types.Where(t => t.Is<T>())
                    .Select(t => t.GetConstructor(args.Select(x => x.GetType()).ToArray()))
                    .FirstOrDefault(ObjectExtensions.NotNull);

            return c.IsNull() ? GetDefault<T>() : (T)(c.Invoke(args));
            
        }

        public IEnumerable<Type> GetAncestorsOf(Type type) {
            while (type.NotNull() &&
                   type.BaseType.NotNull()) {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }

        public IEnumerable<Type> GetAncestorsOf<T>() {
            return GetAncestorsOf(typeof (T));
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

        public bool IsGeneric<T>(Type generic) {
            if (generic.IsGenericTypeDefinition)
            return
                GetAncestorsOf<T>()
                    .FirstOrDefault(
                        x => x.IsGenericType &&
                             x.GetGenericTypeDefinition() == generic
                    ).NotNull();
            return false;
        }

        public Type CreateGeneric(Type generic, params Type[] genericParameters) {
            if (generic.IsGenericTypeDefinition)
                return generic.MakeGenericType(genericParameters);

            throw new Exception("Type provided is not an open generic");
        }

        #endregion

        private static TY Convert<T, TY>(object source) {
            //This fixes a bug in the .Net Framework - 
            //Enables casting from object to a compatible type in a PCL
            var p = Expression.Parameter(typeof (T));
            var converted = Expression.Convert(p, typeof (TY));
            var m = Expression.Lambda<Func<T, TY>>(converted, p).Compile();
            return m((T) source);
        }

        public IEnumerable<Assembly> GetAssemblies() {
            var files = DeferedServices.FileSystem.First().BaseDirectory.GetFiles();
            return this.Try(tp =>
                files
                .Where(x => x.Name.EndsIn("dll", "exe"))
                .Select(
                    x => {
                        var n = x.Name.RemoveExtension();
                        return new AssemblyName { Name = n };
                    }
                ).Select(x => x.Try(y => Assembly.Load(y.ToString())).Act())
                .Where(ObjectExtensions.NotNull)
                .ToArray()
            ).Catch<Exception>((tp, ex) => new[] {Assembly.GetExecutingAssembly()})
            .Act();
        }

        public IEnumerable<Type> GetTypes(Filter<Type> predicate = null) {
            TypeCache = 
                Assemblies
                .SelectMany(x => x.Try(
                    y => predicate.NotNull() ?
                        y.GetExportedTypes().Where(predicate.AsFunc()) :
                        y.GetExportedTypes()).Act()).ToArray();
            return TypeCache;
        }
    }
}