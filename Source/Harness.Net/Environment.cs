using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Harness.Framework;

namespace Harness.Net {
    public class Environment<T> : EnvironmentBase<T> {
        

        public override async Task<IEnumerable<Assembly>> GetAssemblies(string extensionsPath = null) {
            extensionsPath = extensionsPath ?? Environment.CurrentDirectory + @"\Extensions\";
            
            if (Directory.Exists(extensionsPath))
                await 
                Directory.EnumerateFiles(
                    extensionsPath, "*.dll", SearchOption.AllDirectories
                ).EachAsync(x => x.Try(Assembly.LoadFrom).Invoke());

            AssemblyCache = AppDomain.CurrentDomain.GetAssemblies();
            return AssemblyCache;
        }

        
        public override async Task<IEnumerable<Type>> GetTypes(string extensionsPath = null) {
            TypeCache = (AssemblyCache ?? await GetAssemblies(extensionsPath)).SelectMany(x => x.Try(y => y.ExportedTypes).Invoke());
            return TypeCache;
        }
    }
}