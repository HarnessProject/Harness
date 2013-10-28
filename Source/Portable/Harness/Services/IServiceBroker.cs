using System.Collections.Generic;
using System.Linq;

namespace Harness.Services {
    public interface IServiceBroker : IDependency {
        T ServiceFor<T, TY>(TY context);
    }

    public abstract class ServiceBroker : IServiceBroker {
        #region IServiceBroker Members

        public T ServiceFor<T, TY>(TY context) {
            var providers = Application.Resolve<IEnumerable<ServiceProvider<T>>>();
            return providers.EvaluateAll(context).FirstOrDefault();
        }

        #endregion
    }
}