using System;
using System.Composition;
using System.Composition.Dependencies;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class UnitTest3
    {
        public class Type1 : IDependency {
            public int Item1 = 1;
            public int Item3 = 3;
        }

        public class Type2 : IDependency {
            public int Item2 = 2;
            public int Item4 = 4;
        }

        [TestMethod]
        public void TestMethod1() {
            Provider.Start(new FrameworkEnvironment());
            var d = new Type1();
            var e = new Type2();
            

            var g = new ObjectGraft<Type1,Type2>();
            
            g.GraftProperty(x => x.Item1, x => x.Item2);
            g.GraftProperty(x => x.Item3, x => x.Item4);

            g.Graft(d, e);

            Assert.AreEqual(e.Item2, 1);
            Assert.AreEqual(e.Item4, 3);
        }

        [TestMethod]
        public void ContainerTests() {
            Provider.Start(new FrameworkEnvironment());

            var deps = Provider.GetAll<IDependency>();

            Assert.IsTrue(deps.Any());
        }
    }
}
