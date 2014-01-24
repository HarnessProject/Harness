using System;
using System.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    public interface IAmInterface { }
    public class AmClass : IAmInterface { }

    public class AnotherClass
    {
        public static implicit operator AnotherClass(AmClass cl)
        {
            return new AnotherClass();
        }

        public static implicit operator AmClass(AnotherClass cl)
        {
            return new AmClass();
        }
    }

    [TestClass]
    public class DesignTests
    {
        protected TY ExpressionConvert<TY>(Expression<Func<object>> source)
        {
           
            var converted = Expression.Convert(source.Body, typeof (TY));
            return Expression.Lambda<Func<TY>>(converted).Compile()();
        }

        protected TY ReflectedConvert<TY>(Expression<Func<object>> o)
        {
            var t = this;
            var m = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(x => x.Name.Contains("ExpressionConvert"));
            Assert.IsNotNull(m, "Cannot locate method to reflect");
            return (TY)m.MakeGenericMethod(typeof (TY))
                .Invoke(this, new [] {o});

        }

        [TestMethod]
        public void FastestCastingMethod()
        {
            var cl = new AmClass();
            IAmInterface i = null;
            AnotherClass i2 = null;
            AmClass i3 = null;

            var standardElapsed = Metrics.TimeAction(() =>
            {
                i = (IAmInterface) cl;
                i2 = (AnotherClass) cl;
                i3 = (AmClass) i2;
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            
            var expressionElapsed = Metrics.TimeAction(() =>
            {
                i = ExpressionConvert<IAmInterface>(() => cl);
                i2 = ExpressionConvert<AnotherClass>(() => cl);
                i3 = ExpressionConvert<AmClass>(() => i2);
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var reflectedElapsed = Metrics.TimeAction(() =>
            {
                i = ReflectedConvert<IAmInterface>(() => cl);
                i2 = ReflectedConvert<AnotherClass>(() => cl);
                i3 = ReflectedConvert<AmClass>(() => i2);
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var dynamicElapsed = Metrics.TimeAction(() =>
            {
                i = cl.As<IAmInterface>();
                i2 = cl.As<AnotherClass>();
                i3 = i2.As<AmClass>();
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            Assert.IsTrue(
                standardElapsed < reflectedElapsed && reflectedElapsed < expressionElapsed && expressionElapsed < dynamicElapsed, 
                "Dynamic: " + dynamicElapsed.TotalMilliseconds + 
                ", Expression: " + expressionElapsed.TotalMilliseconds + 
                ", Expression (Reflection): " + reflectedElapsed.TotalMilliseconds +
                ", Standard: " + standardElapsed.TotalMilliseconds);
        }
    }
}
