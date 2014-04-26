using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Harness.Framework;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Caliburn.Micro.Harness
{

    public class Bootstrapper : BootstrapperBase, IDependency {
        public Bootstrapper(bool useApplication = true) : base(useApplication)
        {
            Start();
        }
        
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return Provider.Domain.Assemblies;
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = string.IsNullOrWhiteSpace(key) ?
                Provider.Get(service) :
                Provider.Get(service, key);
            if (instance.NotDefault()) return instance;
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Provider.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            Provider.Dependencies.InjectProperties(instance);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }

        /// <summary>
        /// Initializes the Bootstrapper enough to test the Container.
        /// </summary>
        public void StartInTestMode()
        {
            Configure();
        }
    }
}