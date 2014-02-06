using Autofac.Builder;

namespace System.Composition.Autofac {
    public interface IRegistrationContext {
        void RegisterHandlerForType<T>(Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registration);

      
    }
}