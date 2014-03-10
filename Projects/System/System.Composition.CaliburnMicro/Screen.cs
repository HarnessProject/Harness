using System.Collections;
using System.Composition.Dependencies;
using System.Portable;
using System.Portable.Reflection;
using System.Portable.Runtime;
using System.Reactive;
using System.Reflection;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro {
    public abstract class ModelBase : Screen, ITransientDependency
    {
        protected IScope Scope;

        protected ModelBase() {
            Scope = Provider.Get<IScope>();

            var r = Provider.Get<IReflector>();
            r.GetProperties(this, null).Each<PropertyInfo>(
                x => {

                    if (x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof (IReactive<>))
                        r.Impersonate<IReactive<object>>(x).OnNext += o => NotifyOfPropertyChange(x.Name);

                }
            );
        }

        public T Get<T>() {
            return Scope.Get<T>();
        }

        
    }
}