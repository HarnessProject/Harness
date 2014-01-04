using System.Collections.Generic;

namespace System.Composition.UI.Elements {
    public interface IContainerElement : IElement {
        ICollection<IElement> Contents { get; set; } 
    }
}