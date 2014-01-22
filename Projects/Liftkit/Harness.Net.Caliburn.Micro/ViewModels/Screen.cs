using System;
using System.Composition;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Autofac;

namespace Harness.Net.Caliburn.Micro.ViewModels {
    public abstract class Screen : global::Caliburn.Micro.Screen
    {
        protected IScope Scope;

        protected Screen() {
            Scope = Application.NewAsync().AwaitResult();
        }

        public T Get<T>() {
            return Scope.Container.Obtain<T>();
        }

        protected override void OnDeactivate(bool close) {
            base.OnDeactivate(close);
            if (close) Scope.Dispose();
        }
    }
}