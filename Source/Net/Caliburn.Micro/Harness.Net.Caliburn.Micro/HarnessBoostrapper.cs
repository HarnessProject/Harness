using Autofac;

namespace Harness.Net.CaliburnMicro {
    public class Bootstrapper : AutofacBootstrapper<IShell> {
        protected override void ConfigureContainer(ContainerBuilder builder) {
            Application<Environment>.New(new Environment(false, () => builder));
        }

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();

            //  override namespace naming convention
            EnforceNamespaceConvention = false;
            ////  change our view model base type
            //ViewModelBaseType = typeof(IShell);
        }

        protected override void Configure() {
            base.Configure();
            Application.EnvironmentAs<Net.Environment>().SetContainer(Container);
        }
    }
}