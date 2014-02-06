using System.Contracts;

namespace System.Portable.Events {
    public abstract class Event : IEvent {
        #region IEvent Members

        public object Sender { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

        public IEvent Parent { get; set; }

        public ICancelToken Token { get; set; }

        #endregion
    }
}