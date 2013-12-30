using System.Events;

namespace System.Composition.UI.Elements {
    public interface IInteractiveElement : IElement, INotify<IInteractionEvent> {
        
    }
}