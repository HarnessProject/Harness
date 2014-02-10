using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Portable.Reflection;

namespace System.Composition
{
    public class ObjectGraft<T,TY> {
        protected List<Action<T>> PreGraftActions = new List<Action<T>>();
        protected List<Action<TY>> PostGraftActions = new List<Action<TY>>();
        
        protected List<Action<T,TY>> Actions = new List<Action<T, TY>>(); 
        protected List<Tuple<MemberExpression, MemberExpression>> PropertyGrafts = new List<Tuple<MemberExpression, MemberExpression>>();

        public ObjectGraft() {
            Reflector = Container.Get<IReflector>();
        } 

        public ObjectGraft<T, TY> GraftProperty<TX, TZ>(Expression<Func<T,TX>> leftExpression, Expression<Func<TY,TZ>> rightExpression) {
            PropertyGrafts.Add(new Tuple<MemberExpression, MemberExpression>(leftExpression.Body.As<MemberExpression>(), rightExpression.Body.As<MemberExpression>()));
            return this;
        }

        public ObjectGraft<T, TY> GraftAction(Action<T, TY> action) {
            Actions.Add(action);
            return this;
        } 

        public ObjectGraft<T, TY> BeforeGraft(Action<T> action) {
            PreGraftActions.Add(action);
            return this;
        }

        public ObjectGraft<T, TY> AfterGraft(Action<TY> action) {
            PostGraftActions.Add(action);
            return this;
        }

        

        public IReflector Reflector { get; set; }

        public TY Graft(T t, TY ty) {
            PreGraftActions.Each(x => x(t));
            PropertyGrafts.Each(x => {
                var vL = Reflector.GetPropertyValue(t, x.Item1.Member.Name);
                Reflector.SetPropertyValue(ty, x.Item2.Member.Name, vL);
            });
            Actions.Each(x => x(t,ty));
            PostGraftActions.Each(x => x(ty));
            return ty;
        }

        public IEnumerable<TY> GraftAll(IEnumerable<T> tS, Func<TY> fTy) {
            return tS.Select(x => Graft(x, fTy()));
        }


    }
}
