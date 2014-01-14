using System.Collections.Generic;
using System.Composition;
using System.Dynamic;
using System.Messaging;
using System.Portable.Events;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Portable.Runtime {


    public static class Application 
    {

        #region Static Members
        public static IStrongBox Handle { get; set; } 
        public static IScope Global { get; private set; }
        private static Action<IScope> ScopeInitializer { get; set; }
        
        public static Task InitializeAsync(Action<IScope> scopeInitializer) {
            return Task.Factory.StartNew(() => Initialize(scopeInitializer));
        }

        public static void Initialize(Action<IScope> scopeInitializer) {
            ScopeInitializer = scopeInitializer;
            Global = New();

        }

        public static Task<IScope> NewAsync() {
            return Task<IScope>.Factory.StartNew(New);
        }

        public static IScope New() {
            var x = new Scope();
            ScopeInitializer(x);
            return x;
        }
        #endregion

        public static IDependencyProvider Container()
        {
            return Global.Container;
        }

        public static IEventManager EventMessenger()
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

        public static ITypeProvider TypeProvider() {
            return Global.Container.Obtain<ITypeProvider>();
        }
    }

    public class Scope : IScope {
        #region Instance Members
        public IDictionary<string, object> State { get; private set; } 
        public IDependencyProvider Container { get; set; }
        public IMessengerHub MessengerHub { get; set; }
        public IEventManager EventMessenger { get; set; }
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

