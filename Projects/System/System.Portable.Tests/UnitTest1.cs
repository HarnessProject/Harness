using System;
using System.Composition.Autofac;
using System.Portable.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class TypeProviderTestDotNetAutofac
    {
        public TypeProviderTestDotNetAutofac() {
            App.Initialize(x => {
                x.Container = new AutofacDependencyProvider(new TypeProvider(null));
            });
        }

        [TestMethod]
        public void GetDefaultValueType() {
            int dInt = default(int);
            int dTpInt = App.TypeProvider.GetDefault<int>();

            Assert.AreEqual(dInt, dTpInt);
        }

        [TestMethod]
        public void GetDefaultReferenceType() {
            object o = default(object); //NULL
            object Tpo = App.TypeProvider.GetDefault<object>();

            Assert.AreEqual(o,Tpo);
        }

        [TestMethod]
        public void GetDefaultFromValueType() {
            int dInt = default (int);
            int tpInt = (int)App.TypeProvider.GetDefault(typeof (int));

            Assert.AreEqual(dInt, tpInt);
        }
    }
}
