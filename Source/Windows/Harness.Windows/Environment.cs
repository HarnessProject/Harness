using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;
using Autofac;
using Harness.Framework;

namespace Harness.WinRT {
    public class Environment : EnvironmentBase<IDependency> {
        public Environment(bool buildContainer, Func<ContainerBuilder> containerBuilder)
            : base(buildContainer, containerBuilder) {}

        public IEnumerable<Assembly> Assemblies { get; set; }

        public void SetContainer(IContainer container) {
            Container = container;
        }

        public IEnumerable<Assembly> GetAssemblyList() {
            StorageFolder folder = Package.Current.InstalledLocation;

            var assemblyTypes = new[] {".dll", ".exe"};
            var assemblies = new List<Assembly>();

            IReadOnlyList<StorageFile> files = folder.GetFilesAsync(CommonFileQuery.OrderByName).Wait();

            assemblies.AddRange(
                files
                    .Where(
                        x =>
                            assemblyTypes.Contains(x.FileType)
                    )
                    .Select(
                        file =>
                            new AssemblyName {Name = Path.GetFileNameWithoutExtension(file.Name)}
                    )
                    .Select(
                        a =>
                            a.Try(Assembly.Load).Catch<Exception>((o, e) => null).Invoke()
                    )
                    .Where(x => x != null)
                );

            return assemblies;
        }

        public override IEnumerable<Type> GetTypes(string extensionsPath = null) {
            if (Assemblies == null) Assemblies = GetAssemblyList();
            return Assemblies.SelectMany(x => x.ExportedTypes);
        }

        public override IEnumerable<Assembly> GetAssemblies(string extensionsPath = null) {
            return GetAssemblyList();
        }
    }
}