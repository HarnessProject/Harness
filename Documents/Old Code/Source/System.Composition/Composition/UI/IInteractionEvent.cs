using System.Events;

namespace System.Composition.UI {
    public interface IInteractionEvent : IEvent {
        InteractionType Type { get; }
        InteractionModifier Modifier { get; }
    }

    public interface IInteractionEvent<T> : IInteractionEvent, IEvent<T>  {
       
        
    }
}