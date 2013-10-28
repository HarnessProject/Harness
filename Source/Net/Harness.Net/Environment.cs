using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Harness.Net {
    public class Environment<T> : EnvironmentBase<T> {
        public Environment(bool buildContainer = true, Func<ContainerBuilder> builder = null)
            : base(buildContainer, builder) {}

        public void SetContainer(IContainer container) {
            Container = container;
        }

        public override IEnumerable<Assembly> GetAssemblies(string extensionsPath = null) {
            extensionsPath = extensionsPath ?? Environment.CurrentDirectory + @"\Extensions\";
            IEnumerable<Assembly> assemblies =
                Directory.EnumerateFiles(extensionsPath, "*.dll", SearchOption.AllDirectories)
                    .Select(Assembly.LoadFrom);
            AssemblyCache = AppDomain.CurrentDomain.GetAssemblies();
            return AssemblyCache;
        }

        public override IEnumerable<Type> GetTypes(string extensionsPath = null) {
            TypeCache = GetAssemblies(extensionsPath).SelectMany(x => x.ExportedTypes);
            return TypeCache;
        }
    }
}