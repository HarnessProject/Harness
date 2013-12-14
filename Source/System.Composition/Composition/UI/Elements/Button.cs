using System;
using System.Linq;
using System.Messaging;
using System.Text;

namespace System.Composition.UI.Elements
{
    public abstract class InteractiveElement : IInteractiveElement {
        protected InteractiveElement() {
            Context = new BindingContext();
        }
        public void Subscribe<TX>(Action<TX> handler)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(Action<IPropertyChangeEvent> handler)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string propertyName, Action<IPropertyChangeEvent> handler)
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public Guid UniqueName { get; set; }
        public BindingContext Context { get; set; }

        public void Subscribe(Action<IInteractionEvent> handler)
        {
            throw new NotImplementedException();
        }

        public abstract void Initialize();
    }

    public abstract class Button : InteractiveElement
    {
        public override void Initialize()
        {
            
        }
    }
}
