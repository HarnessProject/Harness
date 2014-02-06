using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetFxBugTest {
    public static class TestExtensions
    {
        public static TY ExpressionConvert<TY>(Expression<Func<object>> source)
        {
            var converted = Expression.Convert(source.Body, typeof(TY));
            return Expression.Lambda<Func<TY>>(converted).Compile()();
        }

        public static TY PclExpressionConvert<TY>(Expression<Func<object>> source, Type t) {
            var unfunked = Expression.Convert(source.Body, t); // I resisted my urge to name this unf***ed;
            var converted = Expression.Convert(unfunked, typeof(TY));
            return Expression.Lambda<Func<TY>>(converted).Compile()();
        }

        public static TY ReflectedConvert<TY>(Expression<Func<object>> o)
        {
            var m = typeof(TestExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name.Contains("ExpressionConvert"));
            return (TY)m.MakeGenericMethod(typeof(TY)).Invoke(null, new[] { o });
        }

        public static TY PclReflectedConvert<TY>(Expression<Func<object>> o, Type t) {
            var m = typeof(TestExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name.Contains("PclExpressionConvert"));
            return (TY)m.MakeGenericMethod(typeof(TY)).Invoke(null, new object[] { o, t });
        }

        public static TY FailingCast<TY>(this object o) {
            return ReflectedConvert<TY>(() => o);
        }

        public static TY WorkingCast<TY>(this object o) {
            return PclReflectedConvert<TY>(() => o, o.GetType());
        }

        public static TY CastToObjectGenericCast<T, TY>(this T o) {
            return (TY) (object) o;
        }

        public static TY ContrainedGenericCast<T, TY>(this T o) where T : TY {
            return (TY)o;
        }

        public static TY Func<T,TY>(this T o) where TY : class {
            return o as TY;
        }

        public static TY Cast<TY>(this object o) {
            return (TY) o;
        }

        public static TY DynamicCast<TY>(this object o) {
            return (TY)(dynamic) o;
        }

        public static TY StaticCast<TY>(object o) {
            return (TY) o;
        }
    }
}