using System;
using System.Collections.Generic;
using System.Composition;
using Autofac;

namespace Harness.Autofac {
    
    public static class Extensions
    {
        public static IContainer AutofacContainer(this IScope scope) {
            return scope.Container.As<AutofacDependencyContainer>().Container;
        }

        public static IScope CreateChildScope(this IScope scope) {
            var newScope = new Scope() {
                Container = new AutofacDependencyContainer(scope.AutofacContainer()),
                EventMessenger = scope.EventMessenger,
                MessengerHub = scope.MessengerHub,
            };
            scope.State.Each(x => newScope.State.Add(x));
            return newScope;
        }
    }

    
}