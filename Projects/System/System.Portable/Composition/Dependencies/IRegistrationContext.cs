namespace System.Composition.Dependencies {
    public interface IRegistrationContext {
        void RegisterHandlerForType<T>(Action<IDependencyRegistration> registration);
    }
}