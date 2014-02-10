using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Portable.Reflection;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Autofac;
using IModule = System.Composition.Autofac.IModule;

namespace System.Composition.Autofac {
    public class AutofacContainerFactory : IFactory<IContainer> {
       

        protected Func<Type, bool> Requirements<T>()
        {
            return x => x.Is<T>() && x.IsPublic && !x.IsAbstract && !x.IsInterface; // Who'd a THUNK
        }

        

        public Task<IContainer> CreateAsync() {
            return this.AsTask(x => x.Create());
        }

       
        public IContainer Create() {
            
            var container = CreateContainerBuilder();
            return container == null ? null : container.Build();
        }

        public IContainer Create(params object[] args)
        {
            return Create();
        }
        public AutofacContainerFactory(ITypeProvider provider = null) {
            TypeProvider = provider;
        }

        public ContainerBuilder Builder { get; set; }
       
        public ITypeProvider TypeProvider { get; set; }

        public Task<ContainerBuilder> CreateContainerBuilderAsync() {
            return this.AsTask(x => x.CreateContainerBuilder());
        }
        public ContainerBuilder CreateContainerBuilder(ContainerBuilder builder = null) {
            if (TypeProvider.IsNull()) return null;

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
                .Each((x, cntxt) => x.Register(cntxt.TypeProvider, cntxt.Builder), new {Builder = builder, TypeProvider});

            var iDependencies = ts.Where(x => Requirements<IDependency>()(x)).ToArray();
            
            var suppressionAttributes = 
                iDependencies.SelectMany(
                    x => {
                        var i = 0;
                        return x.GetCustomAttributes(typeof (SuppressDependencyAttribute), true)
                            .Cast<SuppressDependencyAttribute>()
                            .WhereNotDefault()
                            .Select(y => new RegisteredSuppression {OwnerType = x, SuppressionType = y.Type, ScoreSeed = i++})
                            .OrderBy(y => y.Score);
                    }
                ).ToArray();

            iDependencies.Each((type, c) => {
                var suppresed = suppressionAttributes.Where(x => x.SuppressionType == type);
                if (suppresed.FirstOrDefault(x => x.OwnerType != type).NotNull()) return;
 
                var register = builder.RegisterType(type);

                register = register.AsSelf().AsImplementedInterfaces().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

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