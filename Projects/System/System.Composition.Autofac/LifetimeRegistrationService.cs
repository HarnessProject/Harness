using System.Composition.Dependencies;

namespace System.Composition.Autofac {
    public class LifetimeRegistrationService : IAttachToRegistration<IDependency> {
        #region IComponentRegistrationService<IDependency> Members

        public void AttachToRegistration(IRegistrationContext context) {
            context.RegisterHandlerForType<ISingletonDependency>(x => x.AsSingleton());
            context.RegisterHandlerForType<ITransientDependency>(x => x.AsTransient());
        }

        #endregion
    }
}