using System;
using System.Collections.Generic;
using System.Events;
using System.Linq;
using System.Messaging;
using System.Portable.Events;
using System.Text;

namespace System.Composition
{
    public interface INotifyPropertyChange : INotify<IPropertyChangeEvent>
    {
        void Subscribe(string propertyName, Action<IPropertyChangeEvent> handler);
        
    }
}
