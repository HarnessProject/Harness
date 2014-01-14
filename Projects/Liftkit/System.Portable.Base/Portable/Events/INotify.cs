using System.Contracts;

namespace System.Portable.Events
{
    public abstract class NotificationEvent : Event {
        public string Icon { get; set; }
        protected NotificationEvent(object sender, IEvent parent, string icon, string title, string description, ICancelToken token) {
            Icon = icon;
            Token = token;
            Parent = parent;
            TimeStamp = DateTime.Now;
            Description = description;
            Title = title;
            Sender = sender;
        }

    }

    public interface INotify {
        void Notify(NotificationEvent eEvent);
        void NotifyAsync(NotificationEvent eEvent);
    }
}
