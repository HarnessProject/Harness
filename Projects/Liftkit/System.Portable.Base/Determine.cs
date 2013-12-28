//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace System {
//    public static class Determine {
//        public static bool If<T>(this T target, Expression<Func<T, bool>> condition, Action<T> thenAction = null, Action<T> elseAction = null) where T : class {
//            var result = condition.Compile()(target);
//            if (!result) return false;
//            if (thenAction != null) thenAction(target);
//            else if (elseAction != null) elseAction(target);
//            return true;
//        }

//        public static Expression<Func<T, bool>> If<T>(this Expression<Func<T, bool>> func) {
//            return func;
//        }
        

//        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> func, Expression<Func<T, bool>> nFunc) {
//            return x => func.Compile().Invoke(x) && nFunc.Compile().Invoke(x);
//        }

     

//        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> func, Expression<Func<T, bool>> nFunc) {
//            return x => func.Compile().Invoke(x) || nFunc.Compile().Invoke(x);
//        }

//        public static bool When<T>(this Expression<Func<T, bool>> func, T value) {
//            return func.Compile().Try(x => x(value)).Catch<Exception>((y, ex) => false).Invoke();
//        }


//        public static Func<T, bool> And<T>(this bool result) {
//            return t => result;
//        }



//        public static Expression<Func<T, bool>> Then<T>(this Expression<Func<T, bool>> action, params Action<T>[] actions) {
//            return x => action.Compile().Invoke(x) ? actions.Each(y => y(x)).Return(action) : ;
//        }

//        public static Expression<Func<T, bool>> Else<T>(this Expression<Func<T, bool>> func, Action<T> action) {
//            return x => {
//                bool r = If(func).Compile().Invoke(x).And;

//                if (!r) action(x);

//                return r;
//            };
//        }

//        public static bool False<T>(this Expression<Func<T, bool>> func, T target) {
//            return func.Result(target) == false;
//        }

//        public static ProtectedInvocation<T, TY> Try<T, TY>(this T obj, Func<T, TY> tTry) {
//            return new ProtectedInvocation<T, TY>(obj).Try(tTry);
//        }

//        public static T When<T>(this T t, Func<T,bool> condition, Action<T> action) {
//            if (condition(t)) action(t);
//            return t;
//        }
//    }
//}