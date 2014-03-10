using System.Collections;
using System.Collections.Generic;
using System.Composition.Dependencies;
using System.Composition.Providers;
using System.Linq;
using System.Portable.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.GeneratedFactories;
using IModule = System.Composition.Autofac.IModule;

namespace System.Composition.Autofac {
    public class AutofacContainerFactory : IFactory<IContainer> {
       
        protected bool Requirements<T>(Type x)
        {
            return x.Is<T>() && 
                   x.IsPublic && 
                   !x.IsAbstract && 
                   !x.IsInterface; // Who'd a THUNK
        }

        public IContainer Create(dynamic context) {
            TypeProvider = context.TypeProvider;
            var container = CreateContainerBuilder();
            return container == null ? null : container.Build();
        }
        

        public ContainerBuilder Builder { get; set; }
       
        public ITypeProvider TypeProvider { get; set; }

        public ContainerBuilder CreateContainerBuilder(ContainerBuilder builder = null) {
            if (TypeProvider.IsNull()) return null;

            var builderContainer = Builder ?? new ContainerBuilder();
            
            var ts = TypeProvider.Types.ToArray();

            var iComponents = ts.Where(Requirements<IAttachToRegistration<IDependency>>);
            var iBuilders = ts.Where(Requirements<IRegisterDependencies>);
            var iModules = ts.Where(Requirements<IModule>);

            iComponents.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            iModules.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            iBuilders.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());

            var container = builderContainer.Build();
            builder = builder ?? new ContainerBuilder();
            var context = new RegistrationContext();

            container
                .Resolve<IEnumerable<IAttachToRegistration<IDependency>>>()
                .Each(x => x.AttachToRegistration(context));

            container
                .Resolve<IEnumerable<IModule>>()
                .Each(x => builder.RegisterModule(x));

            container
                .Resolve<IEnumerable<IRegisterDependencies>>()
                .Each(x => 
                    x.Register(
                        TypeProvider, 
                        new AutofacDependencyRegistrar(builder, TypeProvider)
                    ));
            
            var iDependencies = ts.Where(Requirements<IDependency>).ToArray();
            
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

            iDependencies.Each(type => {
                var suppresed = suppressionAttributes.Where(x => x.SuppressionType.Is(type));
                if (suppresed.FirstOrDefault(x => x.OwnerType != type).NotNull()) return;
 
                var register = 
                    new AutofacDependencyRegistrar(builder, TypeProvider)
                    .Register(type)
                    .AsAny()
                    .InjectProperties(true);
                            
                var handlers = context.HandlersFor(type);
                handlers.Each(x => x(register));
            });

            builder.RegisterType<IScope>()
                   .InstancePerDependency();
            builder.RegisterInstance(TypeProvider)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            container.Dispose();
            Builder = builder;
            return builder;
        }

        public void Dispose() {
            
        }
    }
}