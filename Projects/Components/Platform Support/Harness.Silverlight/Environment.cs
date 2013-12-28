using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Harness.Silverlight
{
    public class Environment : IEnvironment
    {
        public IContainer Container { get; set; }

        public Environment(bool buildContainer = true,
            Func<ContainerBuilder> containerBuilder = null)
        {

            var builder = containerBuilder != null ? BuildContainer(containerBuilder) : BuildContainer(() => new ContainerBuilder());

            //app.PackageManager = new PackageManager(app.BuildRepository(), app.ExtensionsFolder);

            //Harness Framework types...
            builder.Register<IApplication>(c => Application.CurrentApplication).SingleInstance();

            if (buildContainer) Container = builder.Build();
        }

        public IEnumerable<Type> GetTypes()
        {
            var extensionsPath = System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Extensions";
            if (!Directory.Exists(extensionsPath)) Directory.CreateDirectory(extensionsPath);
            Directory.EnumerateFiles(extensionsPath, "*.dll", SearchOption.AllDirectories).ToList().ForEach(a => Assembly.Load(a));
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetExportedTypes());
        }
        public ContainerBuilder BuildContainer(Func<ContainerBuilder> deferedBuilder)
        {

            var builder = deferedBuilder();
            var types = GetTypes();

            var iModuleInfo = typeof(IModule);
            var iDependencyInfo = typeof(IDependency);

            var iDependencies =
                types
                    .Select(
                        x => x
                    )
                    .Where(
                        t => iDependencyInfo.IsAssignableFrom(t) && t.IsPublic && !t.IsAbstract && !t.IsInterface
                    );

            foreach (var type in iDependencies)
            {
                if (iModuleInfo.IsAssignableFrom(type))
                {
                    builder.RegisterModule(Activator.CreateInstance(type) as IModule);
                    continue;
                }

                var register =
                    builder
                        .RegisterType(type)
                        .InstancePerLifetimeScope();
                
                register.AsSelf();

                var interfaces =
                    type.GetInterfaces()
                        .Where(iDependencyInfo.IsAssignableFrom);

                foreach (var i in interfaces)
                {
                    register.As(i);
                    if (typeof(ISingletonDependency).IsAssignableFrom(i))
                        register = register.SingleInstance();
                    if (typeof(ITransientDependency).IsAssignableFrom(i))
                        register = register.InstancePerDependency();
                }
            }

            return builder;
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
