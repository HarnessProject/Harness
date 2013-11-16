using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Harness.Events;
using Harness.Framework;

namespace Harness.Autofac
{
    public class AutofacServiceLocator : 
        global::Autofac.Extras.CommonServiceLocator.AutofacServiceLocator, 
        IServiceLocator {
        public ILifetimeScope Container { get; set; }

        public AutofacServiceLocator(ILifetimeScope container) : base(container) {
            Container = container;
        }

        public bool GetImplementation<T>(Action<T> action) where T : IServiceLocator {
            return 
                this.Try(x => {
                    if (typeof (T).Is<AutofacServiceLocator>()) {
                        action(x.As<T>());
                        return true;
                    }
                    return false;
                }).Catch<Exception>((x,ex) => false)
                .InvokeAsync()
                .AwaitResult();
        }

        public void Dispose() {
            
        }
    }

    public class AutofacApplicationFactory : IApplicationFactory {
        protected Func<Type, bool> Requirements<T>()
        {
            return x => Q.If<Type>(y => y.Is<T>()).And(y => y.IsPublic && !y.IsAbstract && !y.IsInterface)(x);
        }

        public async Task CreateAsync(IEnvironment environment, bool finalize = true) {
            Finalize = finalize;
            var container = await CreateContainerAsync(environment);
            if (!finalize) return;
            FinalizeServiceLocator(container);
        }

       
        public void Create(IEnvironment environment, bool finalize = true) {
            Finalize = finalize;
            var container = CreateContainer(environment);

            if (!finalize) return;
            FinalizeServiceLocator(container);
        }

        private void FinalizeServiceLocator(ILifetimeScope container) {
            Ready = true;
            X.SetServiceLocator(new AutofacServiceLocator(container));
        }

        public ContainerBuilder Builder { get; set; }
        public bool Finalize { get; set; }
        public bool Ready { get; set; }

        public bool EnsureReady() {
            if (Ready) return true;
            if (!Finalize) FinalizeServiceLocator(Builder.Build());
            else return Finalize;
        }
        protected Task<IContainer> CreateContainerAsync(IEnvironment environment) {
            return this.AsTask(x => x.CreateContainer(environment));
        }
        protected IContainer CreateContainer(IEnvironment environment) {
           
            var builderContainer = Builder ?? new ContainerBuilder();
            
            Type[] ts = environment.TypeCache.ToArray();

            IEnumerable<Type> iComponents = ts.Where(x => Requirements<IComponentRegistrationService<IDependency>>()(x));
            IEnumerable<Type> iBuilders = ts.Where(x => Requirements<IContainerBuilderService>()(x));
            IEnumerable<Type> iModules = ts.Where(x => Requirements<IModule>()(x));

             iComponents.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
             iModules.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
             iBuilders.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());

            var container = builderContainer.Build();
            var builder = new ContainerBuilder();
            var context = new RegistrationContext();

             container
                .Resolve<IEnumerable<IComponentRegistrationService<IDependency>>>()
                .Each((x, c) => x.AttachToRegistration(c), context);

             container
                .Resolve<IEnumerable<IModule>>()
                .Each((x, bldr) => bldr.RegisterModule(x), builder);

             container
                .Resolve<IEnumerable<IContainerBuilderService>>()
                .Each((x, cntxt) => x.AttachToBuilder(cntxt.Environment, cntxt.Builder), new {Builder = builder, X.Environment});

            IEnumerable<Type> iDependencies = ts.Where(x => Requirements<IDependency>()(x));
             iDependencies.Each((type, c) => {
                var register = builder.RegisterType(type);

                register.AsSelf().AsImplementedInterfaces().PropertiesAutowired();

                var handlers = c.HandlersFor(type);
                handlers.Each(x => x(register));
             }, context);

            builder.RegisterType<IScope>().InstancePerDependency();
            
            container.Dispose();
            Builder = builder;
            return Finalize ? builder.Build() : null;
        }

        public void Dispose() {
            
        }
    }
}
