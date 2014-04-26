using Autofac;
using Autofac.Harness;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Autofac.Harness {
    
    public static class Extensions
    {
        public static ILifetimeScope AutofacContainer(this IDependencyProvider scope) {
            return scope.AsType<AutofacDependencyProvider>().Container;
        }

        
    }

    
}