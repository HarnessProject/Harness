using System;
using System.Runtime.Environment;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Caliburn.Micro.Autofac;
using Harness;
using Harness.Autofac;
using Harness.Framework;

namespace Harness.Net.Caliburn.Micro {
    public class Bootstrapper : AutofacBootstrapper<IShell> {
        protected override void ConfigureContainer(ContainerBuilder builder) {
            this.FactoryFor<IContainer>().As<AutofacContainerFactory>(x => x.TypeProvider = new TypeProvider()).CreateContainerBuilder(builder);
        }

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

    }
}