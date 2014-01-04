using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Portable.Events {

    public delegate void DelegateAction(object tObject);
    public delegate void DelegateAction<in T>(T tObject);
    public delegate bool DelegateFilter(object tObject);
    public delegate bool DelegateFilter<in T>(T tObject);
    
    public class DelegatePipeline {
        protected HashSet<RegisteredAction> Actions { get; set; }
        

        public DelegatePipeline() {
            Actions = new HashSet<RegisteredAction>();
        }

        public Guid AddDelegate<T>(DelegateAction<T> action, DelegateFilter<T> filter = null) {
            var compiledAction = ((Expression<DelegateAction>) ( x => action(x.As<T>()) )).Compile();
            var compiledFilter = ((Expression<DelegateFilter>) ( x => filter.IsNull() || filter(x.As<T>()) )).Compile();
            var newHandler = new RegisteredAction() {
                Pipeline = this,
                Id = Guid.NewGuid(),
                Filter = compiledFilter,
                Handler = compiledAction,
                TargetType = typeof(T)
            };
            Actions.Add(newHandler);
            return newHandler.Id;
        }

        public void RemoveDelegate(Guid id) {
            Actions.RemoveWhere(x => x.Id == id);
        }

        protected void DefaultAction(object tObject) {}

        public async Task Process<T>(T tObject) {
            await this.AsTask(t => {
                var h =
                Actions
                    .Where(x => x.TargetType.Is(tObject.GetType()))
                    .Where(x => x.Filter(tObject))
                    .Aggregate(
                        new DelegateAction(DefaultAction),
                        (action, actionHandler) => action + (obj => actionHandler.Handler(obj)));
                var e = ((Expression<Action>)(() => h(tObject))).Compile();
                e();
            });
        }
    }
}