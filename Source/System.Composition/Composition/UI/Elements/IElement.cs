using System.Events;

namespace System.Composition.UI.Elements {
    public interface IElement : INotify
    {
        //<summary>A moniker used to reference a UI Element</summary>
        string Name { get; set; }
        //<summary>A unique identifier for a UI Element</summary>
        Guid UniqueName { get; set; }
        BindingContext Context { get; set; }

    }
}