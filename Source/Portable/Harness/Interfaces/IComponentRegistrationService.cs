namespace Harness {
    public interface IComponentRegistrationService<TY> {
        void AttachToRegistration(IRegistrationContext context);
    }
}