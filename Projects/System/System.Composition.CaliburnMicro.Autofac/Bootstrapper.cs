using System.Composition.Autofac;
using System.Composition.CaliburnMicro.Autofac;
using System.Portable;
using System.Portable.Runtime;
using Autofac;
using Autofac.Core;

namespace System.Composition.CaliburnMicro {
    public class Bootstrapper : AutofacBootstrapper {
        //public bool Configured { get; set; }
        protected override void ConfigureContainer(ContainerBuilder builder) {
            new AutofacContainerFactory {TypeProvider = TypeProvider.Instance}
                .CreateContainerBuilder(builder);
        }

       

        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
            
        }

        protected override void Configure() {
            base.Configure();
            Provider.Start(TypeProvider.Instance, new { Container } );
        }
    }
}