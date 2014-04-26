using System;

namespace Harness.Framework.Interfaces {
    public interface IRegistrationContext {
        void RegistrationHandlerForType<T>(Action<IDependencyRegistration> registration);
    }
}