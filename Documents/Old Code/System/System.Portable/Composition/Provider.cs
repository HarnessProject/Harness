#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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
using System.Composition.Providers;
using System.Composition.Reflection;
using System.Linq.Expressions;
using System.Portable.Reflection;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

#endregion

namespace System.Composition {

    public interface IExtention {}
    internal class ExtentionContainer : IExtention { }

    public static class DeferedServices {

        public static Defered<IFileSystemLayout> FileSystem { get; set; }
        public static Defered<IDependencyProvider> Dependencies { get; set; }
        public static Defered<IReflector> Reflector { get; set; } 

        public static void DependencyProvider(ITypeProvider provider) {
            Dependencies = Defered<IDependencyProvider>.Create(t => t.FactoryFor<IDependencyProvider>().Create(t), provider);

        }

        public static void ReflectorInstance(ITypeProvider provider) {
            Reflector = Defered<IReflector>.Create(t => t.Create<IReflector>(), provider);
        }
    }

    public static class Provider {


        

        public static dynamic Settings { get; private set; }
        public static ITypeProvider Types { get; private set; }
        public static IEnvironment Environment { get; private set; }
        public static IDependencyProvider Dependencies { get { return DeferedServices.Dependencies.Result(); }}
        public static IReflector Reflector { get { return DeferedServices.Reflector.Result(); } }
        public static dynamic State { get; private set; }

        static Provider() {
            
            Types = TypeProvider.Instance;
            

            Settings = new JObject();
            State = new JObject();

            DeferedServices.DependencyProvider(Types);
            DeferedServices.ReflectorInstance(Types);

        }

        private static void Set<T>(Expression<Func<T>> property, T value) {
            property.Body.As<MemberExpression>().Member.As<PropertyInfo>().SetValue(null, value, null);
        }

        public static IExtention Get() {
            return new ExtentionContainer();
        }

        public static object Get(Type serviceType) {
            return Dependencies.Get(serviceType);
        }

        public static object Get(Type serviceType, string key) {
            return Dependencies.Get(serviceType, key);
        }

        public static IEnumerable<object> GetAll(Type serviceType) {
            return Dependencies.GetAll(serviceType);
        }

        public static T Get<T>() {
            return Dependencies.Get<T>();
        }

        public static T Get<T>(string key) {
            return Dependencies.Get<T>(key);
        }

        public static IEnumerable<T> GetAll<T>() {
            return Dependencies.GetAll<T>();
        }
    }
}