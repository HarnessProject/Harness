using System;
using System.Collections.Generic;
using System.Composition;
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
        public IScope Scope { get; set; }

        public EventMessenger(IMessengerHub messenger, IScope scope) {
            Messenger = messenger;
            Scope = scope;
            Scope.Container.GetAllInstances<IHandle<IEvent>>().Each(x => Handle<IEvent>(x.Handle));
        }

        public void Handle<T>(Action<T> handler) where T : class, IEvent {
            Messenger.Subscribe<T>(m => handler.AsTask(x => x(m)), m => !m.Canceled.IsCancellationRequested, true);
        }

        public Task Trigger<T>(T evnt) where T : class, IEvent {
            return Messenger.AsTask(x => x.Publish(evnt));

        }


    }
}
