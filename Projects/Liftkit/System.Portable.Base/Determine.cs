using System.Collections.Generic;
using System.Linq.Expressions;

namespace System
{
    public static class Determine
    {
        public static bool If<T>(this T target, Func<T, bool> condition, Action<T> thenAction = null, Action<T> elseAction = null) where T : class
        {
            var result = condition(target);
            if (!result) return false;
            if (thenAction != null) thenAction(target);
            else if (elseAction != null) elseAction(target);
            return true;
        }

        public static Func<T, bool> If<T>(this Func<T, bool> func)
        {
            return func;
        }


        public static Func<T, bool> And<T>(this Func<T, bool> func, Func<T, bool> nFunc)
        {
            return x => func.Invoke(x) && nFunc.Invoke(x);
        }



        public static Func<T, bool> Or<T>(this Func<T, bool> func, Func<T, bool> nFunc)
        {
            return x => func.Invoke(x) || nFunc.Invoke(x);
        }

        public static bool When<T>(this Func<T, bool> func, T value)
        {
            return func.Try(x => x(value)).Catch<Exception>((y, ex) => false).Invoke();
        }


        public static Func<T, bool> And<T>(this bool result)
        {
            return t => result;
        }



        public static Func<T, bool> Then<T>(this Func<T, bool> action, params Action<T>[] actions) {
            return x =>
            {
                var r = action.Invoke(x);
                if (r)
                {
                    actions.Each(y => y(x));
                    
                }
                return r;
            };
        }

        public static Func<T, bool> Else<T>(this Func<T, bool> func, Action<T> action)
        {
            return x =>
            {
                bool r = If(func).Invoke(x);

                if (!r) action(x);

                return r;
            };
        }

        public static bool False<T>(this Func<T, bool> func, T target)
        {
            return func(target) == false;
        }

        

        public static T When<T>(this T t, Func<T, bool> condition, Action<T> action)
        {
            if (condition(t)) action(t);
            return t;
        }
    }
}