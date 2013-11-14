using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Autofac;


using Harness.Events;
using Harness.Framework;

namespace Harness.Autofac
{
    public class AutofacServiceLocator : 
        global::Autofac.Extras.CommonServiceLocator.AutofacServiceLocator, 
        IServiceLocator {
        public AutofacServiceLocator(IComponentContext container) : base(container) { }
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

        public async Task CreateAsync(IEnvironment environment) {
            var result = await CreateContainerAsync(environment);
            X.SetServiceLocator(new AutofacServiceLocator(result));
        }

        public ContainerBuilder Builder { get; set; }
        protected async Task<IContainer> CreateContainerAsync(IEnvironment environment) {
           
            var builderContainer = Builder ?? new ContainerBuilder();
            Type[] ts = environment.TypeCache.ToArray();

            IEnumerable<Type> iComponents = ts.Where(x => Requirements<IComponentRegistrationService<IDependency>>()(x));
            IEnumerable<Type> iBuilders = ts.Where(x => Requirements<IContainerBuilderService>()(x));
            IEnumerable<Type> iModules = ts.Where(x => Requirements<IModule>()(x));

            await iComponents.EachAsync(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            await iModules.EachAsync(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            await iBuilders.EachAsync(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());

            var container = builderContainer.Build();
            var builder = new ContainerBuilder();
            var context = new RegistrationContext();

            await container
                .Resolve<IEnumerable<IComponentRegistrationService<IDependency>>>()
                .EachAsync((x, c) => x.AttachToRegistration(c), context);

            await container
                .Resolve<IEnumerable<IModule>>()
                .EachAsync((x, bldr) => bldr.RegisterModule(x), builder);

            await container
                .Resolve<IEnumerable<IContainerBuilderService>>()
                .EachAsync((x, cntxt) => x.AttachToBuilder(cntxt.Environment, cntxt.Builder), new {Builder = builder, X.Environment});

            IEnumerable<Type> iDependencies = ts.Where(x => Requirements<IDependency>()(x));
            await iDependencies.EachAsync((type, c) => {
                var register = builder.RegisterType(type);

                register.AsSelf();
                register.AsImplementedInterfaces();

                var handlers = c.HandlersFor(type);
                handlers.EachAsync(x => x(register)).Await();
            }, context);

            builder.Register<IScope>(
                c =>
                    new Scope() {
                        ServiceLocator = new AutofacServiceLocator(c.Resolve<ILifetimeScope>()),
                        Environment = X.Environment
                    }.Action(x => {
                        x.Dispatcher = x.ServiceLocator.GetInstance<IDispatch>();
                        x.EventManager = x.ServiceLocator.GetInstance<IEventManager>();
                    })
            ).InstancePerDependency();
            
            container.Dispose();
            return builder.Build();
        }

        public void Dispose() {
            
        }
    }
}
