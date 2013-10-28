namespace Harness.Web {
    public class WebComponentRegistrationService : IComponentRegistrationService<IDependency> {
        #region IComponentRegistrationService<IDependency> Members

        public void AttachToRegistration(IRegistrationContext context) {
            context.RegisterHandlerForType<IController>(r => r.InstancePerHttpRequest());
            context.RegisterHandlerForType<IApiController>(r => r.InstancePerApiRequest());
        }

        #endregion
    }
}