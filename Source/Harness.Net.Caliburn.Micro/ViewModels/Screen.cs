using System;
using System.Composition;
using System.Tasks;
using Autofac;

namespace Harness.Net.Caliburn.Micro.ViewModels {
    public abstract class Screen : global::Caliburn.Micro.Screen
    {
        protected IScope Scope;

        protected Screen() {
            Scope = ApplicationScope.NewScopeAsync().AwaitResult();
        }

        public T Get<T>() {
            return Scope.Container.GetInstance<T>();
        }

        protected override void OnDeactivate(bool close) {
            base.OnDeactivate(close);
            if (close) Scope.Dispose();
        }
    }
}