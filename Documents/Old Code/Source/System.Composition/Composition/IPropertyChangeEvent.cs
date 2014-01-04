using System.Events;

namespace System.Composition {
    public interface IPropertyChangeEvent : IEvent<IProperty> { }
    public interface IPropertyChangeEvent<T> : IEvent<IProperty<T>>, IPropertyChangeEvent { }
}