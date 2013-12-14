using System.Collections.Generic;


namespace System {
    public static class Determine {
        public static bool If<T>(this T target, Func<T, bool> condition, Action<T> thenAction = null, Action<T> elseAction = null) where T : class {
            var result = condition(target);
            if (!result) return false;
            if (thenAction != null) thenAction(target);
            else if (elseAction != null) elseAction(target);
            return true;
        }

        public static Func<T, bool> If<T>(this Func<T, bool> func) {
            return func;
        }
        

        public static Func<T, bool> And<T>(this Func<T, bool> func, Func<T, bool> nFunc) {
            return x => func(x) && nFunc(x);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> func, Func<T, bool> nFunc) {
            return x => func(x) || nFunc(x);
        }

        public static bool Result<T>(this Func<T, bool> func, T target = default(T)) {
            return func.Try(x => x(target)).Catch<Exception>((y, ex) => false).Invoke();
        }

        

        

        public static Func<T, bool> Then<T>(this Func<T, bool> action, params Action<T>[] actions) {
            return x => {
                bool r = If(action).Result(x);
                if (r) actions.Each(y => y(x));

                return r;
            };
        }

        public static Func<T, bool> Else<T>(this Func<T, bool> func, Action<T> action) {
            return x => {
                bool r = If(func).Result(x);

                if (!r) action(x);

                return r;
            };
        }

        public static bool False<T>(this Func<T, bool> func, T target) {
            return func.Result(target) == false;
        }

        public static ProtectedInvocation<T, TY> Try<T, TY>(this T obj, Func<T, TY> tTry) {
            return new ProtectedInvocation<T, TY>(obj).Try(tTry);
        }
    }
}