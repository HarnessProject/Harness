using System.Collections.Generic;

namespace System {
    public static class Filter {
        public static bool If<T>(this T target, Filter<T> condition, Action<T> thenAction = null, Action<T> elseAction = null) where T : class {
            bool result = condition(target);
            if (!result) return false;
            if (thenAction != null) thenAction(target);
            else if (elseAction != null) elseAction(target);
            return true;
        }

        public static Filter<T> If<T>(Filter<T> func) {
            return func;
        }

        public static Filter<T> And<T>(this Filter<T> func, Filter<T> nFunc) {
            return x => func(x) && nFunc(x);
        }

        public static Filter<T> Or<T>(this Filter<T> func, Filter<T> nFunc) {
            return x => func(x) || nFunc(x);
        }

        public static bool Result<T>(this Filter<T> func, T target = default(T)) {
            return func.Try(x => x(target)).Catch<Exception>((y, ex) => false).Act();
        }

        public static bool True<T>(this Filter<T> func, T target = default(T)) {
            return func.Result(target);
        }

        public static Filter<T> Then<T>(this Filter<T> action, Action<T> then) {
            return x => {
                bool r = If(action).True(x);
                if (r) then(x);

                return r;
            };
        }

        public static Filter<T> Then<T>(this Filter<T> action, params Action<T>[] actions) {
            return x => {
                bool r = If(action).True(x);
                if (r) actions.Each(y => y(x));

                return r;
            };
        }

        public static Filter<T> Else<T>(this Filter<T> func, Action<T> action) {
            return x => {
                bool r = If(func).True(x);

                if (!r) action(x);

                return r;
            };
        }

        public static bool False<T>(this Filter<T> func, T target) {
            return func.Result(target) == false;
        }

        
    }
}