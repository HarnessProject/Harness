using System.ComponentModel;
using System.Composition.Annotations;
using System.Linq.Expressions;
using System.Portable.Runtime.Reflection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Composition {
    public abstract class NotifyPropertyChange : Notifier<IPropertyChangeEvent>, INotifyPropertyChange, INotifyPropertyChanged {

        public IReflector Reflector { get; set; }

        public void ChangeProperty<TY>(Expression<Func<TY>> prop, TY newValue) {
            var member = (MemberExpression) prop.Body;
            var p = ((PropertyInfo) member.Member);
            
            
            Reflector.Try(x => {
                var old = x.GetPropertyValue(this, p.Name);
                x.SetPropertyValue(this, p.Name, newValue);
                PropertyChange(p.Name, old, newValue);
                return true;
            }).Catch<Exception>((x, ex) => {
                throw new InvalidOperationException(String.Format("{0}, Set Failed.",p.Name), ex);
            }).Act();
            
        }

        #region INotifyPropertyChange Members
        public void PropertyChange(string property, object oldvalue, object newValue) {
            Notify(new PropertyChangeEvent {PropertyName = property, OldValue = oldvalue, NewValue = newValue});
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}