#region ApacheLicense

// System.Portable.Base
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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

using System.Collections.Generic;
using System.Contracts;
using System.Linq;
using System.Portable.App.Dynamic;
using System.Portable.Runtime.Dynamic;
using System.Threading.Tasks;

#endregion

namespace System.Portable.Runtime {
    public delegate void DelegateAction(object tObject);

    public delegate void DelegateAction<in T>(T tObject);

    public delegate bool DelegateFilter(object tObject);

    public delegate bool DelegateFilter<in T>(T tObject);

    /// <summary>
    ///     Manages the execution of one or more delegates registered to perform an action on an object provided at run time.
    ///     Notes:
    ///     We take a pragmatic, and likely unecessary, approach to flattening the actions/filters into a single delegate.
    ///     We compile our lambdas into expressions and call methods whenever possible.
    ///     Finally we combine the DelegateActions into a single Action and pass it to a method which will invoke it.
    ///     We do the same to global filters, which will prevent actions from running.
    ///     As well as providing individual action filters.
    ///     It is among the simplest pipeline execution systems available and is 100% portable.
    ///     We use it for events messages. Our messaging system is TinyMessenger, and although a capable messenging hub,
    ///     we need our events to fire as fast as possible, and all constituant members of an event solution need to be
    ///     present.
    ///     We also need fast and easy registration.
    ///     TODO: Create Examples of usage beyond EventManager
    /// </summary>
    public class DelegatePipeline : IDependency {
        public DelegatePipeline() {
            Actions = new HashSet<RegisteredAction>();
            Invoker = App.Container.Get<IDynamicInvoker>();
        }

        protected HashSet<RegisteredAction> Actions { get; set; }
        protected HashSet<RegisteredFilter> Filters { get; set; }
        protected IDynamicInvoker Invoker { get; set; }

        public Guid AddDelegate<T>(DelegateAction<T> action, DelegateFilter<T> filter = null) {
            var compiledAction = CreateAction(action);
            var compiledFilter = CreateFilter(filter);
            var newHandler = new RegisteredAction {
                Id = Guid.NewGuid(),
                Filter = compiledFilter,
                Handler = compiledAction,
                TargetType = typeof (T)
            };
            Actions.Add(newHandler);
            return newHandler.Id;
        }

        public void RemoveDelegate(Guid id) {
            Actions.RemoveWhere(x => x.Id == id);
        }

        public Guid AddFilter<T>(DelegateFilter<T> filter) {
            var compiledFilter = CreateFilter(filter);
            var newFilter = new RegisteredFilter {
                Id = Guid.NewGuid(),
                Filter = compiledFilter,
                TargetType = typeof (T)
            };
            Filters.Add(newFilter);
            return newFilter.Id;
        }

        public void RemoveFilter(Guid id) {
            Filters.RemoveWhere(x => x.Id == id);
        }

        protected void DefaultAction(object tObject) {}

        protected bool DefaultFilter(object tObject) {
            return true;
        }

        public async Task Process<T>(T tObject) {
            await this.AsTask(o => {
                if (!Invoker.InvokeReturn(CreateFilter(tObject)).As<bool>()) return;
                Invoker.InvokeAction(CreateAction(tObject));
            });
        }

        protected Func<bool> CreateFilter<T>(T tObject) {
            var f =
                Filters
                    .Where(y => y.TargetType.Is<T>())
                    .Aggregate(
                        new DelegateFilter(DefaultFilter),
                        (filter, filterHandler) => filter + filterHandler.Filter
                    );

            return () => ExecuteFilter(f, tObject);
        }

        protected Action CreateAction<T>(T tObject) {
            var h =
                Actions
                    .Where(x => x.TargetType.Is<T>())
                    .Where(x => x.Filter(tObject))
                    .Aggregate(
                        new DelegateAction(DefaultAction),
                        (action, actionHandler) => action + actionHandler.Handler
                    );
            return () => ExecuteAction(h, tObject);
        }

        protected DelegateAction CreateAction<T>(DelegateAction<T> action) {
            return x => {
                var t = typeof (T);
                if (t.Is<ICancel>() && x.As<ICancel>().Token.Canceled) return;
                if (t.Is<IContinue>() && !x.As<IContinue>().Token.Continue) return;
                action(x.As<T>());
            }; // THUNK
        }

        protected DelegateFilter CreateFilter<T>(DelegateFilter<T> filter) {
            return x => filter.IsNull() || filter(x.As<T>());
        }

        protected void ExecuteAction(DelegateAction action, object tObject) {
            Invoker.InvokeAction(action, tObject);
        }

        protected bool ExecuteFilter(DelegateFilter filter, object tObject) {
            return Invoker.InvokeReturn(filter, tObject).As<bool>();
        }
    }
}