#region ApacheLicense
// From the Harness Project
// Caliburn.Micro.Harness.Tests
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
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
        public IShell Shell { get; set; }
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
        IShell Shell { get; set; }
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
            Assert.IsNotNull(fancy.Shell, TestMessages.IsNullOrDefault<FancyService>("fancy.Shell"));
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