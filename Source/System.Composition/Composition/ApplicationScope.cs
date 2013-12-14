using System.Collections.Generic;
using System.Events;
using System.Messaging;
using System.Threading.Tasks;

namespace System.Composition {


    public sealed class ApplicationScope  : IScope
    {
        #region Static Members
        public static ApplicationScope Global { get; set; }
        private static Action<ApplicationScope> ScopeFactory { get; set; }
        
        public static Task InitializeAsync(Action<ApplicationScope> scopeFactory) {
            return Task.Factory.StartNew(() => Initialize(scopeFactory));
        }

        public static void Initialize(Action<ApplicationScope> scopeFactory) {
            ScopeFactory = scopeFactory;
            var x = new ApplicationScope();
            ScopeFactory(x);
            Global = x;
        }

        public static Task<ApplicationScope> NewScopeAsync() {
            return Task<ApplicationScope>.Factory.StartNew(NewScope);
        }

        public static ApplicationScope NewScope() {
            var x = new ApplicationScope();
            ScopeFactory(x);
            return x;
        }
        #endregion

        #region Instance Members
        public IDictionary<string, object> State { get; private set; } 
        public IDependencyContainer Container { get; set; }
        public IMessengerHub MessengerHub { get; set; }
        public IEventMessenger EventMessenger { get; set; }
        #endregion

        private ApplicationScope() {
            State = new Dictionary<string, object>();
        }

        public void Dispose() {
           Container.Dispose();
           GC.SuppressFinalize(this); 
        }


        
    }

    
}
