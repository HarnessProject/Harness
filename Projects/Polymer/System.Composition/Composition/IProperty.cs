using System.Events;
using System.Messaging;

namespace System.Composition {
    public interface IProperty : INotify<IPropertyChangeEvent>
    {
        //<summary>A moniker used to reference a UI Element</summary>
        string Name { get; set; }
        object Value { get; set; }
    }

    public interface IProperty<T> : INotify<IPropertyChangeEvent<T>>, IProperty {
        new T Value { get; set; }
    }

    public interface IEventProperty : INotify<IEvent>, IProperty {}
    public interface IEventProperty<T> : INotify<IEvent<T>>, IEventProperty { }

    public class ElementProperty : IProperty, IDependency {

        public IEventMessenger EventMessenger { get; set; }
        
        public void Subscribe(Action<IPropertyChangeEvent> handler) {
            EventMessenger.Handle(handler);
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class ElementProperty<T> : ElementProperty, IProperty<T> {
        public void Subscribe(Action<IPropertyChangeEvent<T>> handler) {
           EventMessenger.Handle(handler);
        }

        public new T Value { get; set; }
    }
}