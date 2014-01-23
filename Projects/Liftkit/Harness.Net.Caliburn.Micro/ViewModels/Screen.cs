using System;
using System.Composition;
using System.Portable;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Autofac;

namespace Harness.Net.Caliburn.Micro.ViewModels {
    public abstract class Screen : global::Caliburn.Micro.Screen
    {
        protected IScope Scope;

        protected Screen()
        {
            Scope = App.NewScope();
        }

        public T Get<T>() {
            return Scope.Container.Get<T>();
        }

        protected override void OnDeactivate(bool close) {
            base.OnDeactivate(close);
            if (close) Scope.Dispose();
        }
    }
}