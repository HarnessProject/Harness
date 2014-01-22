﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Portable.Runtime;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Portable.Runtime.Environment {
    public class TypeProvider : ITypeProvider {
       

        protected IEnumerable<Assembly> AssemblyCache { get; set; }
        protected IEnumerable<Type> TypeCache { get; set; }
        public virtual async Task<IEnumerable<Assembly>> GetAssemblies(string extensionsPath = null) {
            extensionsPath = extensionsPath ?? AppDomain.CurrentDomain.BaseDirectory;
            
            if (Directory.Exists(extensionsPath))
                await 
                Directory.EnumerateFiles(
                    extensionsPath, "*.dll", SearchOption.AllDirectories
                ).EachAsync(x => x.Try(Assembly.LoadFrom).Invoke());

            AssemblyCache = AppDomain.CurrentDomain.GetAssemblies();
            return AssemblyCache;
        }

        
        public virtual async Task<IEnumerable<Type>> GetTypes(Func<Type, bool> predicate, string extensionsPath) {
            TypeCache = TypeCache.NotNull() ? TypeCache : (AssemblyCache ?? await GetAssemblies(extensionsPath))
                .SelectMany(x => x.Try(
                    y => predicate.NotNull() ? y.ExportedTypes.Where(predicate) : y.ExportedTypes
                ).Invoke());
            return TypeCache;
        }

        public IEnumerable<Type> GetTypes(Func<Type, bool> predicate = null) {
            return GetTypes(predicate, null).AwaitResult();
        }

        public TypeProvider() {
            

            GetTypes();
        }

        public TypeProvider(string extensionPath) {
           

            GetTypes(null, extensionPath).Await();
        }

        public IEnumerable<Assembly> Assemblies { get { return AssemblyCache; } }
        public IEnumerable<Type> Types { get { return TypeCache; } }
        
    }

    public static class TypeProviders {
        public static ITypeProvider DefaultInstance { get; set; }
        public static IFactory<T> FactoryFor<T>(this object o, Action<IFactory<T>> initalizer = null) {
            DefaultInstance = DefaultInstance ?? new TypeProvider();
            var factoryType = DefaultInstance.Types.FirstOrDefault(x => x.Is<IFactory<T>>());
            if (factoryType.NotNull()) return default(IFactory<T>);

            var factory = (IFactory<T>)Activator.CreateInstance(factoryType);
            initalizer.NotNull(x => x(factory));
            return factory;
        }
    }
}