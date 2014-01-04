using System.Collections.Generic;
using System.Events;
using System.Linq;

namespace System.Composition.UI.Elements {
    public class BindingContext {
        public IList<IEventProperty> Events { get; private set; }
        public IList<IProperty> Properties { get; private set; }
        public BindingContext() {
            Events = new List<IEventProperty>();
            Properties = new List<IProperty>();
        }
    }
}