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

using System.Portable.Runtime.Dynamic;
using System.Threading.Tasks;

#endregion

namespace System.Portable.Runtime {
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
    public class Pipeline : IDependency {
        public Pipeline(IDynamicInvoker invoker) {
            Actions = new HashSet<RegisteredAction>();
            Invoker = invoker;
        }

        protected HashSet<RegisteredAction> Actions { get; set; }
        protected HashSet<RegisteredFilter> Filters { get; set; }
        protected IDynamicInvoker Invoker { get; set; }

        public Guid AddDelegate<T>(Action<T> action, Filter<T> filter = null) {
            var compiledAction = CreateActionWrapper(action);
            var compiledFilter = CreateFilterWrapper(filter);
            var newHandler = new RegisteredAction {
                Id = Guid.NewGuid(),
                WrappedFilter = compiledFilter,
                WrappedAction = compiledAction,
                TargetType = typeof (T)
            };
            Actions.Add(newHandler);
            return newHandler.Id;
        }

        public void RemoveDelegate(Guid id) {
            Actions.RemoveWhere(x => x.Id == id);
        }

        public Guid AddFilter<T>(Filter<T> filter) {
            var compiledFilter = CreateFilterWrapper(filter);
            var newFilter = new RegisteredFilter {
                Id = Guid.NewGuid(),
                WrappedFilter = compiledFilter,
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
                        new WrappedFilter(DefaultFilter),
                        (filter, filterHandler) => filter + filterHandler.WrappedFilter
                    );

            return () => ExecuteFilter(f, tObject);
        }

        protected Action CreateAction<T>(T tObject) {
            var h =
                Actions
                    .Where(x => x.TargetType.Is<T>())
                    .Where(x => x.WrappedFilter(tObject))
                    .Aggregate(
                        new WrappedAction(DefaultAction),
                        (action, actionHandler) => action + actionHandler.WrappedAction
                    );
            return () => ExecuteAction(h, tObject);
        }

        protected WrappedAction CreateActionWrapper<T>(Action<T> action) {
            return x => {
                var t = typeof (T);
                if (t.Is<ICancel>() && x.As<ICancel>().Token.Canceled) return;
                if (t.Is<IContinue>() && !x.As<IContinue>().Token.Continue) return;
                action(x.As<T>());
            }; // THUNK
        }

        protected WrappedFilter CreateFilterWrapper<T>(Filter<T> filter) {
            return x => filter.IsNull() || filter(x.As<T>());
        }

        protected void ExecuteAction(WrappedAction action, object tObject) {
            Invoker.InvokeAction(action, tObject);
        }

        protected bool ExecuteFilter(WrappedFilter wrappedFilter, object tObject) {
            return Invoker.InvokeReturn(wrappedFilter, tObject).As<bool>();
        }
    }
}