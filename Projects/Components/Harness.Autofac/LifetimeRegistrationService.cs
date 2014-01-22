using System.Composition;
using System.Portable.Runtime;

namespace Harness.Autofac {
    public class LifetimeRegistrationService : IComponentRegistrationService<IDependency> {
        #region IComponentRegistrationService<IDependency> Members

        public void AttachToRegistration(IRegistrationContext context) {
            context.RegisterHandlerForType<ISingletonDependency>(x => x.SingleInstance());
            context.RegisterHandlerForType<ITransientDependency>(x => x.InstancePerDependency());
        }

        #endregion
    }
}