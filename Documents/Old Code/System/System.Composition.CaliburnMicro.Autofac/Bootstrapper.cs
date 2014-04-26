using System.Composition.Autofac;
using System.Composition.CaliburnMicro.Autofac;
using Autofac;

namespace System.Composition.CaliburnMicro
{
    public class Bootstrapper : AutofacBootstrapper {
        //public bool Configured { get; set; }
        protected override void ConfigureContainer(ContainerBuilder builder) {
            
        }

       

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
            
        }

        protected override void Configure() {
            base.Configure();
            Provider.Set(() => Provider.Dependencies, new AutofacDependencyProvider(Container));
        }
    }
}