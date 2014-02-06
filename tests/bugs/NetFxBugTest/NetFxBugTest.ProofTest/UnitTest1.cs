using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetFxBugTest.ProofTest
{
    public interface IAmInterface { }
    public class AmClass : IAmInterface { }

    public class AnotherClass
    {
        public static explicit operator AnotherClass(AmClass cl)
        {
            return new AnotherClass();
        }

        public static explicit operator AmClass(AnotherClass cl)
        {
            return new AmClass();
        }
    }

    public static class Extensions
    {
        public static TY Test<TY>(this object o)
        {
            return (TY)o;
        }
    }

    [TestClass]
    public class DesignTests
    {
        

        [TestMethod]
        public void NonPclCastingMetrics()
        {
            var cl = new AmClass();
            IAmInterface i = null;
            AnotherClass i2 = null;
            AmClass i3 = null;
           
            var standardElapsed = Metrics.TimeAction(() =>
            {
                i = (IAmInterface)cl;
                i2 = (AnotherClass)cl;
                i3 = (AmClass)i2;
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var expressionElapsed = Metrics.TimeAction(() =>
            {
                i = TestExtensions.ExpressionConvert<IAmInterface>(() => cl);
                i2 = TestExtensions.ExpressionConvert<AnotherClass>(() => cl);
                i3 = TestExtensions.ExpressionConvert<AmClass>(() => i2);
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var reflectedElapsed = Metrics.TimeAction(() =>
            {
                i = TestExtensions.ReflectedConvert<IAmInterface>(() => cl);
                i2 = TestExtensions.ReflectedConvert<AnotherClass>(() => cl);
                i3 = TestExtensions.ReflectedConvert<AmClass>(() => i2);
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var dynamicElapsed = Metrics.TimeAction(() => {
                i = cl.DynamicCast<IAmInterface>();
                i2 = cl.DynamicCast<AnotherClass>();
                i3 = i2.DynamicCast<AmClass>();
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            var workingPclElapsed = Metrics.TimeAction(() =>
            {
                i = cl.WorkingCast<IAmInterface>();
                i2 = cl.WorkingCast<AnotherClass>();
                i3 = i2.WorkingCast<AmClass>();
            });
            Assert.IsNotNull(i, "Can't cast AmClass to IAmInterface");
            Assert.IsNotNull(i2, "Can't cast IAmInterface to AnotherClass");
            Assert.IsNotNull(i3, "Can't cast AnotherClass to AmClass");
            
            Assert.Fail(
            
                "\n" +
                "Working PCL Cast: " + workingPclElapsed.TotalMilliseconds + "\n" +
                "Dynamic: " + dynamicElapsed.TotalMilliseconds + "\n" +
                "Expression: " + expressionElapsed.TotalMilliseconds + "\n" +
                "Expression (Reflection): " + reflectedElapsed.TotalMilliseconds + "\n" +
                "Standard: " + standardElapsed.TotalMilliseconds);
        }

        [TestMethod]
        public void PclCastByExtensionMethod() {
            AmClass cl = new AmClass();
            var pclExCastFail = false;
            Exception e = null;
            try
            {
                var asObject = cl.Cast<AnotherClass>();
                e = new Exception("Cast Successfull");
            }
            catch (InvalidCastException ex)
            {
                pclExCastFail = true;
                e = ex;
            }

            Assert.Fail("\n" +
                "PCL Extension Method Cast Failed: " + pclExCastFail + "\n" +
                "Exception Message: " + e.Message);
        }

        [TestMethod]
        public void PclCastByStaticMethod() {
            AmClass cl = new AmClass();
            var pclStCastFail = false;
            Exception stEx = null;
            try
            {
                var asObject = TestExtensions.StaticCast<AnotherClass>(cl);
                stEx = new Exception("Cast Successfull");
            }
            catch (InvalidCastException ex)
            {
                pclStCastFail = true;
                stEx = ex;
            }
            Assert.Fail("\n" +
                        "PCL Static Method Cast Failed: " + pclStCastFail + "\n" +
                        "Exception Message: " + stEx.Message);
        }

        [TestMethod]
        public void PclCastInstanceMethod() {
            var pclInstanceCastFail = false;
            Exception iEx = null;
            try
            {
                var asAPcl = new PclClass().AsObject<AnotherPclClass>();
                iEx = new Exception("Cast Successfull");
            }
            catch (InvalidCastException ex)
            {
                pclInstanceCastFail = true;
                iEx = ex;
            }

            Assert.Fail("\n" +
                "PCL Instance Method Cast Failed: " + pclInstanceCastFail + "\n" +
                "Exception Message: " + iEx.Message);
        }

        [TestMethod]
        public void PclCastByDefinedGenericCastToObject() {
            AmClass cl = new AmClass();
            var genericPclCast = false;
            Exception gEx = null;
            try
            {
                var failedDefinedGeneric = cl.CastToObjectGenericCast<AmClass, AnotherClass>();
                gEx = new Exception("Cast Works!!!!!!!");
            }
            catch (InvalidCastException ex)
            {
                genericPclCast = true;
                gEx = ex;
            }

            Assert.Fail("\n" +
                "PCL Defined Generic To Object Cast failed: " + genericPclCast + "\n" +
                "Exception Message: " + gEx.Message);
        }

        [TestMethod]
        public void PclCastByConstrainedGenericWithAs() {
            var cGenericPclCast = false;
            Exception cgEx = null;
            try {
                //var fail = new AmClass().ContrainedGenericCast<AmClass, AnotherClass>();
                var fail = new AmClass().Func<AmClass,AnotherClass>();
                if (fail == null) throw new InvalidCastException("Value of cast was null");
                cgEx = new Exception("Cast Succeeded!!!");
            }
            catch (InvalidCastException ex)
            {
                cGenericPclCast = true;
                cgEx = ex;
            }

            Assert.Fail("\n" +
                        "PCL Constrained Generic Cast With As Failed:" + cGenericPclCast + "\n" +
                        "Exception Message: " + cgEx.Message);
        }

        [TestMethod]
        public void PclCastByExpressionFromReflectedMethod()
        {
            var workingPclCast = false;
            Exception useless = null;
            try
            {
                var workingPcl = new AmClass().WorkingCast<AnotherClass>();
                useless = new Exception("Cast Works!!!!!!!");
            }
            catch (InvalidCastException ex)
            {
                workingPclCast = true;
                useless = ex;
            }

            var failingPclCast = false;
            Exception usefull = null;
            try
            {
                var failedPcl = new AmClass().FailingCast<AnotherClass>();
                usefull = new Exception("Cast Successfull");
            }
            catch (TargetInvocationException ex)
            {
                failingPclCast = true;
                usefull = ex;
            }

            Assert.Fail("\n" +
                "PCL Working and Failing Reflected Method, Casting By Expression\n" +
                "Working Failed: " + workingPclCast + ":\n" +
                "Casts the object back to it's original type before casting to TY\n" +
                "This is the only way this cast works besides dynamic, which is VERY SLOW\n\n" +
                "The working method is only marginally faster than using the ExpressionConvert method\n" +
                "For some reason invoking ExpressionConvert via reflection is faster than calling it?????\n\n" +
                
                "Failing Failed: " + failingPclCast + ":\n Only casts the object to TY\n" +
                "Exception Message: " + usefull.Message + "\n" +
                "InnerException: " + usefull.InnerException.Message + "\n");
        } 
    }

    
}
