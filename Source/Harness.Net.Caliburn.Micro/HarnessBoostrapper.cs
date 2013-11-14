using Autofac;
using Caliburn.Micro.Autofac;
using Harness;
using Harness.Framework;

namespace Harness.Net.Caliburn.Micro {
    public class Bootstrapper : AutofacBootstrapper<IShell> {
        protected override void ConfigureContainer(ContainerBuilder builder) {
           Core.Application<Environment<IDependency>>.New(new Environment<IDependency>(false, () => builder));
        }

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

        protected override void Configure() {
            base.Configure();
            Harness.X.Environment.As<Environment<IDependency>>().SetContainer(Container);
        }
    }
}