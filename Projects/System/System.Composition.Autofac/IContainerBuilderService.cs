using Autofac;

namespace System.Composition.Autofac {
    
    public static class Extensions
    {
        public static ILifetimeScope AutofacContainer(this IScope scope) {
            return scope.DependencyProvider.As<AutofacDependencyProvider>().Container;
        }

        public static IScope CreateChildScope(this IScope scope) {
            var newScope = new Scope(new AutofacDependencyProvider(
                    scope.AutofacContainer().BeginLifetimeScope().As<ILifetimeScope>()
                ));
            foreach (var k in scope.State) newScope.State.Add(k.Key, k.Value);
            return newScope;
        }
    }

    
}