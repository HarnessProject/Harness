using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class ProtectedActionTests
    {
        public ProtectedActionTests() {
            Provider.Start(new FrameworkEnvironment());
        }

        [TestMethod]
        public void CatchAll() {
            var r = this.Try(x => {
                throw new InvalidDataException();
                return true;
            }).Catch<Exception>((x, ex) => false)
            .Act();

            Assert.IsFalse(r);
        }

        [TestMethod]
        public void CatchSpecific() {
            var r =
                this.Try(
                    x => {
                        throw new InvalidDataException();
                        return 1;
                    }
                ).Catch<InvalidDataException>((x, ex) => 2)
                .Catch<Exception>((x, ex) => 3)
                .Act();

            Assert.AreEqual(r, 2);
        }

        [TestMethod]
        public void CatchAllFinally() {
            int r2 = 0;
            var r = this.Try(x => {
                throw new InvalidDataException();
                return 1;
            }).Catch<Exception>((x, ex) => 2)
            .Finally(x => r2 = x*2)
            .Act();

            Assert.AreEqual(r, 2);
            Assert.AreEqual(r2, 4);
        }
    }
}
