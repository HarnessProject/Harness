using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Portable.Runtime;
using System.Tasks;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Harness.Autofac
{
    public class AutofacDependencyContainer : IDependencyContainer {
        public IContainer Container { get; set; }

        public AutofacDependencyContainer(ITypeProvider environment) {
            Container = new AutofacContainerFactory() {TypeProvider = environment}.Create();
        }

        public AutofacDependencyContainer(IContainer container) {
            Container = container;
        }

        public bool GetImplementation<T>(Action<T> action) where T : IDependencyContainer {
            return this.ExecuteIf<AutofacDependencyContainer>(x => action(x.As<T>()));
        }

        public Task<bool> GetImplementationAsync<T>(Action<T> action) where T : IDependencyContainer {
            return this.AsTask(x => GetImplementation(action));
        }

        public void Dispose() {
            Container.Dispose();
        }

        public object GetService(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType, string key) {
            return Container.ResolveNamed(key, serviceType);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType) {
            return ((IEnumerable)Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType))).Cast<Object>();
        }

        public TService GetInstance<TService>() {
            return Container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string key) {
            return Container.ResolveNamed<TService>(key);
        }

        public IEnumerable<TService> GetAllInstances<TService>() {
            return Container.Resolve<IEnumerable<TService>>();
        }

        public T GetInstanceOf<T>(string type) {
            return Container.Resolve(Type.GetType(type)).As<T>();
        }
    }

    public class AutofacContainerFactory : IFactory<IContainer> {
       

        protected Func<Type, bool> Requirements<T>()
        {
            return x => 
                Determine
                    .If<Type>(y => y.Is<T>())
                    .And(y => y.IsPublic)
                    .And(y => !y.IsAbstract)
                    .And(y => !y.IsInterface)(x);
        }

        

        public Task<IContainer> CreateAsync() {
            return this.AsTask(x => x.Create());
        }

       
        public IContainer Create() {
            
            var container = CreateContainerBuilder();
            return container.Build();
        }

        public AutofacContainerFactory() {
            
        }

        public ContainerBuilder Builder { get; set; }
       
        public ITypeProvider TypeProvider { get; set; }

        public Task<ContainerBuilder> CreateContainerBuilderAsync() {
            return this.AsTask(x => x.CreateContainerBuilder());
        }
        public ContainerBuilder CreateContainerBuilder(ContainerBuilder builder = null) {
           
            var builderContainer = Builder ?? new ContainerBuilder();
            
            var ts = TypeProvider.Types.ToArray();

            var iComponents = ts.Where(x => Requirements<IComponentRegistrationService<IDependency>>()(x));
            var iBuilders = ts.Where(x => Requirements<IRegistrationProvider<ContainerBuilder>>()(x));
            var iModules = ts.Where(x => Requirements<IModule>()(x));

             iComponents.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
             iModules.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
             iBuilders.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());

            var container = builderContainer.Build();
            builder = builder ?? new ContainerBuilder();
            var context = new RegistrationContext();

             container
                .Resolve<IEnumerable<IComponentRegistrationService<IDependency>>>()
                .Each((x, c) => x.AttachToRegistration(c), context);

             container
                .Resolve<IEnumerable<IModule>>()
                .Each((x, bldr) => bldr.RegisterModule(x), builder);

             container
                .Resolve<IEnumerable<IRegistrationProvider<ContainerBuilder>>>()
                .Each((x, cntxt) => x.Register(cntxt.TypeProvider, cntxt.Builder), new {Builder = builder, TypeProvider = TypeProvider});

             var iDependencies = ts.Where(x => Requirements<IDependency>()(x));
             iDependencies.Each((type, c) => {
                var register = builder.RegisterType(type);

                register.AsSelf().AsImplementedInterfaces().PropertiesAutowired();

                var handlers = c.HandlersFor(type);
                handlers.Each(x => x(register));
             }, context);

            builder.RegisterType<IScope>().InstancePerDependency();
            builder.RegisterInstance(TypeProvider).AsImplementedInterfaces().SingleInstance();

            container.Dispose();
            Builder = builder;
            return builder;
        }

        public void Dispose() {
            
        }
    }
}
