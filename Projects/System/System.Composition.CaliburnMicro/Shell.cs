using System.Portable;
using System.Portable.Runtime;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro {
    public abstract class Shell : Conductor<ModelBase>, IShell {
        protected IScope Scope;

        protected Shell()
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