using System;
using System.Runtime.Environment;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Caliburn.Micro.Autofac;
using Harness.Autofac;

namespace Harness.Net.Caliburn.Micro {
    public class Bootstrapper : AutofacBootstrapper<IShell> {
        protected override void ConfigureContainer(ContainerBuilder builder) {
            new AutofacContainerFactory {TypeProvider = new TypeProvider()}.CreateContainerBuilder(builder);
        }

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

        protected override async void Configure() {
            base.Configure();

            await Application.InitializeAsync(x => {
                x.Container = new AutofacServiceLocator(Container);
            });
        }
    }
}