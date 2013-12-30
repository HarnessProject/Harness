﻿using System.Events;
using System.Portable.Events;

namespace System.Composition {
    public interface IPropertyChangeEvent : IEvent {
        string PropertyName { get; }
    }
}