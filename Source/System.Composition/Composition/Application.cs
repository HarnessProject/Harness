using System.Collections.Generic;
using System.Dynamic;
using System.Events;
using System.Messaging;
using System.Threading.Tasks;

namespace System.Composition {


    public static class Application 
    {
        #region Static Members
        public static IScope Global { get; private set; }
        private static Action<IScope> ScopeFactory { get; set; }
        
        public static Task InitializeAsync(Action<IScope> scopeFactory) {
            return Task.Factory.StartNew(() => Initialize(scopeFactory));
        }

        public static void Initialize(Action<IScope> scopeFactory) {
            ScopeFactory = scopeFactory;
            Global = New();

        }

        public static Task<IScope> NewAsync() {
            return Task<IScope>.Factory.StartNew(New);
        }

        public static IScope New() {
            var x = new Scope();
            ScopeFactory(x);
            return x;
        }
        #endregion

        public static IDependencyContainer Container()
        {
            return Global.Container;
        }

        public static IEventMessenger EventMessenger()
        {
            return Global.EventMessenger;
        }

        public static IMessengerHub MessengerHub()
        {
            return Global.MessengerHub;
        }

        public static dynamic State()
        {
            return Global.State;
        }

        public static dynamic State(this IScope scope) {
            return scope.State;
        }
    }

    public class Scope : IScope {
        #region Instance Members
        public IDictionary<string, object> State { get; private set; } 
        public IDependencyContainer Container { get; set; }
        public IMessengerHub MessengerHub { get; set; }
        public IEventMessenger EventMessenger { get; set; }
        #endregion

        public Scope() {
            State = new ExpandoObject();
        }

        public void Dispose() {
           Container.Dispose();
           GC.SuppressFinalize(this); 
        }
    }    
}

