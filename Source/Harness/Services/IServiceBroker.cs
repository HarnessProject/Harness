using System.Collections.Generic;
using System.Linq;
using Harness.Framework;

namespace Harness.Services {
    public interface IServiceBroker : IDependency {
        T ServiceFor<T, TY>(TY context);
    }

    public abstract class ServiceBroker : IServiceBroker {
        #region IServiceBroker Members

        public T ServiceFor<T, TY>(TY context) {
            var providers = X.ServiceLocator.GetAllInstances<IServiceProvider<T>>();
            return providers.AsTask(x => x.EvaluateAll(context).FirstOrDefault()).AwaitResult();
        }

        #endregion
    }
}