using System;
using Autofac.Builder;

namespace Harness.Autofac {
    public interface IRegistrationContext {
        void RegisterHandlerForType<T>(Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registration);

      
    }
}