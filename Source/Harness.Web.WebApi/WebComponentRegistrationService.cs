using System.Web.Http;
using Autofac.Integration.WebApi;
using Autofac;
namespace Harness.Web.WebApi {
    public class WebApiControlerLifetimeRegistrationService : IComponentRegistrationService<IDependency> {
        #region IComponentRegistrationService<IDependency> Members

        public void AttachToRegistration(IRegistrationContext context) {
            context.RegisterHandlerForType<ApiController>(r => r.InstancePerApiRequest());
            context.RegisterHandlerForType<IApiController>(r => r.InstancePerApiRequest());
        }

        #endregion
    }
}