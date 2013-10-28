using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Harness.Framework;

namespace Harness {
    public abstract class EnvironmentBase<T> : IEnvironment {
        protected EnvironmentBase(
            bool buildContainer = true,
            Func<ContainerBuilder> containerBuilder = null) {
            var builder = containerBuilder != null
                ? CreateContainer(containerBuilder)
                : CreateContainer(() => new ContainerBuilder());
            builder.Register(c => Application.CurrentApplication).SingleInstance();

            if (buildContainer) Container = builder.Build();
        }

        #region IEnvironment Members

        public IEnumerable<Assembly> AssemblyCache { get; protected set; }
        public IEnumerable<Type> TypeCache { get; protected set; }
        //public abstract ContainerBuilder BuildContainer(Func<ContainerBuilder> deferedContainer);

        public IContainer Container { get; protected set; }

        public virtual T Resolve<T>() {
            return Container.Resolve<T>();
        }

        public virtual Object Resolve(Type type) {
            return Container.Resolve(type);
        }

        public virtual Object Resolve(string typeName) {
            try {
                Type type = Type.GetType(typeName);
                return Resolve(type);
            }
            catch (Exception ex) {
                throw new DependencyResolutionException(ex.Message, ex);
            }
            //return null;
        }

        #endregion

        public abstract IEnumerable<Assembly> GetAssemblies(string extensionsPath = null);
        public abstract IEnumerable<Type> GetTypes(string extensionsPath = null);

        protected Func<Type, bool> Requirements<T>() {
            return x => Q.If<Type>(y => y.Is<T>()).And(y => !y.IsAbstract || !y.IsInterface)(x);
        }

        public ContainerBuilder CreateContainer(Func<ContainerBuilder> deferedBuilder) {
            ContainerBuilder builder = deferedBuilder();
            IEnumerable<Type> types = GetTypes();

            Type[] ts = types as Type[] ?? types.ToArray();
            var context = new RegistrationContext();

            IEnumerable<Type> iComponents = ts.Where(x => Requirements<IComponentRegistrationService<T>>()(x));
            IEnumerable<Type> iModules = ts.Where(x => Requirements<IModule>()(x));
            IEnumerable<Type> iDependencies = ts.Where(x => Requirements<T>()(x));

            iComponents.EachAsync(
                (component, c) =>
                    Activator.CreateInstance(component)
                    .As<IComponentRegistrationService<T>>()
                    .AttachToRegistration(c),
                context
                ).Await();

            iModules.EachAsync((type, b) => b.RegisterModule(Activator.CreateInstance(type) as IModule), builder)
                .Await();

            iDependencies.EachAsync(
                (type, c) => {
                    var register = builder.RegisterType(type);

                    register.AsSelf();
                    register.AsImplementedInterfaces();

                    c.HandlersFor(type).EachAsync(x => x(register)).Await();
                },
                context
                ).Await();

            return builder;
        }
    }
}