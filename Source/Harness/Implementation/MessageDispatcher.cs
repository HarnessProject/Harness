using System;
using System.Linq;
using System.Threading.Tasks;
using Harness.Framework;

namespace Harness {
    public class MessageDispatcher : IDispatch {
        public LookupTable<Type, MessageHandler> Receivers { get; private set; }
        protected TaskScheduler Scheduler { get; set; }
        public MessageDispatcher() {
            Receivers = new LookupTable<Type, MessageHandler>();
            
        }

        public async Task SendAsync<T>(T message) {
            var targets = Receivers[typeof (T)];
            await targets.EachAsync(x => x(message));
        }

        public void Receive<T>(params MessageHandler<T>[] handlers) where T : class
        {
            
            Receivers.Add(
                typeof(T),
                handlers.Select(x => new MessageHandler(y => x(y as T))).ToArray()
                );
        }
    }
}