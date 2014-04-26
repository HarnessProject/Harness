using Harness.Framework.Interfaces;

namespace Autofac.Harness
{
    public class LifetimeRegistrationService : IAttachToRegistration {
        #region IComponentRegistrationService<IDependency> Members

        public void AttachToRegistration(IRegistrationContext context) {
            context.RegistrationHandlerForType<ISingletonDependency>(x => x.AsSingleton());
            context.RegistrationHandlerForType<ITransientDependency>(x => x.AsTransient());
        }

        #endregion
    }
}