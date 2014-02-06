using System.Portable;
using System.Portable.Runtime;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro {
    public abstract class ModelBase : NotifyPropertyChange, IDependency, INotifyPropertyChangedEx
    {
        protected IScope Scope;

        protected ModelBase()
        {
            Scope = App.NewScope();
        }

        public T Get<T>() {
            return Scope.Container.Get<T>();
        }

        protected void OnDeactivate(bool close) {
           
            if (close) Scope.Dispose();
        }

        public void NotifyOfPropertyChange(string propertyName) {
            IsNotifying = true;

        }

        public void Refresh() {
            throw new NotImplementedException();
        }

        public bool IsNotifying { get; set; }
    }
}