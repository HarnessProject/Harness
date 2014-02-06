#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

#endregion

#region

using System.Messaging;
using System.Portable.Events;
using System.Portable.Messaging;
using System.Portable.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#endregion

namespace System.Portable {
    public static class App {
        public static IStrongBox Handle { get; set; }
        public static IScope Global { get; private set; }
        private static Action<IScope> ScopeInitializer { get; set; }

        public static Task InitializeAsync(Action<IScope> scopeInitializer) {
            return Task.Factory.StartNew(() => Initialize(scopeInitializer));
        }

        public static void Initialize(Action<IScope> scopeInitializer) {
            Handle = new StrongBox<Object>(new object());
            ScopeInitializer = scopeInitializer;
            Global = NewScope();

        }

        public static IScope NewScope() {
            var x = new Scope();
            ScopeInitializer(x);
            return x;
        }

        public static IDependencyProvider Container { get { return Global.Container; } }

        public static IEventManager EventManager { get { return Global.EventManager; } }

        public static IMessengerHub MessengerHub { get { return Global.MessengerHub; } }

        public static ITypeProvider TypeProvider { get { return Global.Container.Get<ITypeProvider>(); } }

        public static dynamic State { get { return Global.State; } }

        public static dynamic ScopeState(this IScope scope) {
            return scope.State;
        }

        
    }
}