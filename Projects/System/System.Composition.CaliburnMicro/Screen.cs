using System.Portable;
using System.Portable.Runtime;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro {
    public abstract class ModelBase : Screen, ITransientDependency
    {
        protected IScope Scope;

        protected ModelBase()
        {
            Scope = App.NewScope();
        }

        public T Get<T>() {
            return Scope.Container.Get<T>();
        }

        
    }
}