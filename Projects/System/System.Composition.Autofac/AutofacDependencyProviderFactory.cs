using System.Composition.Providers;
using System.Portable.Reflection;
using Autofac;

namespace System.Composition.Autofac {
    public class AutofacDependencyProviderFactory : IFactory<IDependencyProvider> {
        public IDependencyProvider Create(dynamic context) {
            return 
                TypeExtensions.Is<ILifetimeScope>(context) ?
                new AutofacDependencyProvider(TypeExtensions.As<ILifetimeScope>(context)): 
                new AutofacDependencyProvider(TypeExtensions.As<ITypeProvider>(context));
        }
    }
}