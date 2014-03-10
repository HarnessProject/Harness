using System.Portable;
using System.Portable.Runtime;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro {
    public abstract class Shell : Conductor<ModelBase>, IShell {
        protected IScope Scope;

        protected Shell() {
            Scope = Provider.Get<IScope>();
        }

        public T Get<T>() {
            return Scope.Get<T>();
        }

    }
}