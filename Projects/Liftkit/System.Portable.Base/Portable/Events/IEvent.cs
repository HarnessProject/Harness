using System.Contracts;
using System.Messaging;
using System.Threading;

namespace System.Portable.Events
{


    /// <summary>
    /// Represents an event. 
    /// This interface should be satisfied to the point that every event fired is loggable.
    /// Senders can implment ICallback T to receive specific callback messages.
    /// </summary>
    public interface IEvent  {
        object Sender { get; }
        string Title { get; }
        string Description { get; }
        DateTime TimeStamp { get; }
        IEvent Parent { get; } 
        ICancelToken Token { get; }
    }
}
