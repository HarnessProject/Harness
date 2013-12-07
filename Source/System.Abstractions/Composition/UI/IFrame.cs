using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Composition.UI
{
    //<summary>A UI Element</summary> 
    public interface IElement
    {
        //<summary>A moniker used to reference a UI Element</summary>
        string Name { get; set; }
        //<summary>A unique identifier for a UI Element</summary>
        Guid UniqueName { get; set; }
    }

    //<summary>A UI Container Element</summary>
    public interface IContainerElement : IElement {
        ICollection<IElement> Contents { get; set; } 
    }

    //<summary A UI Element that will be interacted with by the user</summary>
    public interface IInteractiveElement : IElement {
        event InteractionHandler Interaction;
    }

    public delegate void InteractionHandler(IElement source, InteractionContext context);

    public enum InteractionType {
        Direct,
        Indirect
    }

    public enum DirectInteractionType {
        Click,
        Touch,
        Drag,
        Scroll,
        PinchIn,
        PinchOut,
        MultiTouch
    }

    public class InteractionContext {
        public InteractionType Type { get; set; }
    }
}
