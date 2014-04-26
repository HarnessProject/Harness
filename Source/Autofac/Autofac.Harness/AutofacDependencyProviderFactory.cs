
using System;
using Autofac;
using Autofac.Harness;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;


namespace Autofac.Harness {
    public class AutofacDependencyProviderFactory : IFactory<IDependencyProvider> {
        public IDependencyProvider Create() {

            var container = 
                this.Try(x => Provider.State.$AutofacContainer.AsType<IContainer>())
                .Catch<Exception>((x,ex) => null)
                .Act();

            if (container.NotNull()) return new AutofacDependencyProvider(container.BeginLifetimeScope());
            return new AutofacDependencyProvider();
        }
    }
}