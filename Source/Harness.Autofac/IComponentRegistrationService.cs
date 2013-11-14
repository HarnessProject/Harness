namespace Harness.Autofac {
    public interface IComponentRegistrationService<TY> {
        void AttachToRegistration(IRegistrationContext context);
    }
}