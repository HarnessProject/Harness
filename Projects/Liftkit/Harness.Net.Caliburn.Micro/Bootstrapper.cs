using System;
using System.Collections.Generic;
using System.Portable;
using System.Portable.Runtime;
using System.Portable.Runtime.Environment;
using System.Security.Policy;
using Autofac;
using Caliburn.Micro.Autofac;
using Harness.Autofac;

namespace Harness.Net.Caliburn.Micro {
    public class Bootstrapper : AutofacBootstrapper<IShell> {
        //public bool Configured { get; set; }
        protected override void ConfigureContainer(ContainerBuilder builder) {
            new AutofacContainerFactory {TypeProvider = TypeProvider.Instance}
                .CreateContainerBuilder((ContainerBuilder)builder);
        }

        protected override object GetInstance(Type service, string key)
        {
            //if (!Configured) Configure();
            return base.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            //if (!Configured) Configure();
            return base.GetAllInstances(service);
        }

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
            
        }

        protected override void Configure() {
            base.Configure();

            App.Initialize(x => {
                x.Container = new AutofacDependencyProvider(Container);
            });

            //Configured = true;
        }
    }
}