using System;
using System.Composition;
using System.Composition.Autofac;
using System.Portable.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class TypeProviderTestDotNetAutofac
    {
        public TypeProviderTestDotNetAutofac() {
            Provider.Start(new FrameworkEnvironment());
        }

        [TestMethod]
        public void GetDefaultValueType() {
            int dInt = default(int);
            int dTpInt = Provider.Types.GetDefault<int>();

            Assert.AreEqual(dInt, dTpInt);
        }

        [TestMethod]
        public void GetDefaultReferenceType() {
            object o = default(object); //NULL
            object Tpo = Provider.Types.GetDefault<object>();

            Assert.AreEqual(o,Tpo);
        }

        [TestMethod]
        public void GetDefaultFromValueType() {
            int dInt = default (int);
            int tpInt = (int)Provider.Types.GetDefault(typeof (int));

            Assert.AreEqual(dInt, tpInt);
        }
    }
}
