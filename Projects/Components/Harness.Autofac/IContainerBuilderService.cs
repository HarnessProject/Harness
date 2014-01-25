using System;
using System.Collections.Generic;
using System.Composition;
using System.Portable;
using System.Portable.Runtime;
using Autofac;

namespace Harness.Autofac {
    
    public static class Extensions
    {
        public static IContainer AutofacContainer(this IScope scope) {
            return scope.Container.As<AutofacDependencyProvider>().Container;
        }

        public static IScope CreateChildScope(this IScope scope) {
            var newScope = new Scope() {
                Container = new AutofacDependencyProvider(scope.AutofacContainer()),
                EventManager = scope.EventManager,
                MessengerHub = scope.MessengerHub,
            };
            foreach (var k in scope.State) newScope.State.Add(k.Key, k.Value);
            return newScope;
        }
    }

    
}