using System;

namespace Harness.Framework {
    public static class Q {
        public static bool If<T>(
            this T target, Func<T, bool> condition, Action<T> thenAction = null,
            Action<T> elseAction = null) where T : class {
            bool result = condition(target);
            if (result)
                if (thenAction != null) thenAction(target);
                else if (elseAction != null) elseAction(target);
            return result;
        }

        public static Func<T, bool> If<T>(Func<T, bool> func) {
            return func;
        }

        public static Func<T, bool> And<T>(this Func<T, bool> func, Func<T, bool> nFunc) {
            return x => func(x) && nFunc(x);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> func, Func<T, bool> nFunc) {
            return x => func(x) || nFunc(x);
        }

        public static bool Result<T>(this Func<T, bool> func, T target = default(T)) {
            return func.Try(x => x(target)).Catch<Exception>((y, ex) => false).Invoke() == false;
        }

        public static bool True<T>(this Func<T, bool> func, T target = default(T)) {
            return func.Result(target);
        }

        public static Func<T, bool> Then<T>(this Func<T, bool> action, Action<T> then) {
            return x => {
                bool r = If(action).True(x);
                if (r)
                    then(x);

                return r;
            };
        }

        public static Func<T, bool> Then<T>(this Func<T, bool> action, params Action<T>[] actions) {
            return x => {
                bool r = If(action).True(x);
                if (r)
                    actions.Each(y => y(x));

                return r;
            };
        }

        public static Func<T, bool> Else<T>(this Func<T, bool> func, Action<T> action) {
            return x => {
                bool r = If(func).True(x);

                if (!r)
                    action(x);

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