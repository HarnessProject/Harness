using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Portable.Runtime;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class TypeProviderTests
    {
        [TestMethod]
        public void TypeProviderProvidesAssembliesAndTypes()
        {
            var provider = TypeProvider.Instance;
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).SelectMany(x => x.ExportedTypes);
            
            Assert.IsNotNull(provider.Assemblies, "the TypeProvider has failed to yield any assemblies");
            Assert.IsNotNull(provider.GetTypes(), "The TypeProvider has failed to yield any types");
            Assert.IsNotNull(provider.Types, "The TypeProvider has not cached any types");

            Assert.AreEqual(assemblies.Count(), provider.Assemblies.Count(), "Assembly count mismatch between AppDomain and TypeProvider");
            Assert.AreEqual(types.Count(), provider.Types.Count(), "Type count mismatch between AppDomain and TypeProvider");

        }


    }
}