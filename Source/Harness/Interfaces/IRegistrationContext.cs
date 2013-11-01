using System;
using Autofac;
using Autofac.Builder;

namespace Harness {
    public interface IRegistrationContext {
        void RegisterHandlerForType<T>(Action<IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registration);

        void RegisterContainerBuilder(ContainerBuilder builder, Action<ContainerBuilder> newBuilder);
    }
}