using System;
using System.Linq;
using Autofac;
using Autofac.Harness;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Caliburn.Micro.Harness.Tests
{
    public class FancyService : IFancyService
    {
        public DateTime ParseDate(string date)
        {
            return date.Parse<DateTime>();
        }
    }

    public class PseudoService : ITestService
    {
        public string Name { get { return "Service"; } }

        public bool ServiceOperation()
        {
            return true;
        }
    }

    public interface ITestService : IDependency
    {
        bool ServiceOperation();
    }

    public interface IFancyService : IDependency
    {
        DateTime ParseDate(string date);
    }

    public class TestShell : Shell
    {
    }

    [TestClass]
    public class ProviderTests
    {
        public static Bootstrapper Bootstrapper { get; set; } = new Bootstrapper(false);

        /// <summary>
        ///     Verfies the Provider class initalizes properly, the Bootstrapper starts, 
        ///     and that several adhoc types are resolved properly.
        /// </summary>
        [TestMethod]
        public void ProviderStart()
        {
            Assert.IsNotNull(Bootstrapper);
            
            Assert.IsNotNull(Provider.Dependencies);
            Assert.AreNotEqual(0, Provider.Domain.Types.Count());
            Assert.AreNotEqual(0, Provider.Domain.Assemblies.Count());

            var autofac = Provider.Dependencies.AutofacContainer();
            Assert.IsTrue(autofac.IsRegistered<ITestService>(), TestMessages.NotRegistered<ITestService>() );
            Assert.IsTrue(autofac.IsRegistered<PseudoService>(), TestMessages.NotRegistered<PseudoService>() );
            Assert.IsTrue(autofac.IsRegistered<IFancyService>(), TestMessages.NotRegistered<IFancyService>() );
            Assert.IsTrue(autofac.IsRegistered<FancyService>(), TestMessages.NotRegistered<FancyService>() );

            
            var service = Provider.Get<ITestService>();
            Assert.IsNotNull(service, TestMessages.IsNullOrDefault<ITestService>("service") );
            Assert.IsTrue(service.ServiceOperation(), TestMessages.NotTrue("service.ServiceOperation()") );

            var date = DateTime.Now.Date;
            var fancy = Provider.Get<IFancyService>();
            var parsedDate = fancy.ParseDate(date.Date.ToShortDateString());
            Assert.AreEqual(date, parsedDate, TestMessages.NotEqual("date", "parsedDate"));
        }

        /// <summary>
        ///     Verifies that several well known types are resolved by the Provider.
        /// </summary>
        [TestMethod]
        public void ProviderProvidersTypes()
        {
            Assert.IsNotNull(Provider.Get<IScope>());
            Assert.IsNotNull(Provider.Get<IReflector>());
            Assert.IsNotNull(Provider.Get<IDomainProvider>());
            Assert.IsNotNull(Provider.Get<IShell>());

        }
    }
}