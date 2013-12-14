using System;
using System.Collections.Generic;
using System.Contracts;
using System.Diagnostics.Contracts;
using System.Events;
using System.Linq;
using System.Messaging;
using System.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Events
{
   
    public class EventMessenger : IEventMessenger {
        protected IMessengerHub Messenger { get; set; }
        protected Dictionary<Guid, CancellationToken> TokenState { get; set; } 
        public EventMessenger(IMessengerHub messenger) {
            Messenger = messenger;
            TokenState = new Dictionary<Guid, CancellationToken>();
        }

        public void Handle<T>(Action<T> handler) where T : class, IEvent {
            Messenger.Subscribe<T>(m => handler.AsTask(x => x(m)), m => !m.Token.Canceled, true);
        }

        public ICancelToken Trigger<T>(T evnt) where T : class, IEvent {
            Messenger.AsTask(x => x.Publish(evnt));
            return evnt.Token;
        }


    }
}
