using System;
using Autofac;

namespace Harness.Net.Caliburn.ViewModels {
    public abstract class Screen : Caliburn.Micro.Screen {
        protected ILifetimeScope Scope;

        protected Screen() {
            Scope = Application.EnvironmentAs<Net.Environment>().Container.BeginLifetimeScope(Guid.NewGuid());
        }

        public T Resolve<T>() {
            return Scope.Resolve<T>();
        }

        protected override void OnDeactivate(bool close) {
            base.OnDeactivate(close);
            if (close) Scope.Dispose();
        }
    }
}