using System.Contracts;

namespace System.Portable.Events {
    public abstract class NotificationEvent : Event {
        protected NotificationEvent(object sender, IEvent parent, string title, string description, ICancelToken token) {
            Token = token;
            Parent = parent;
            TimeStamp = DateTime.Now;
            Description = description;
            Title = title;
            Sender = sender;
        }
    }
}