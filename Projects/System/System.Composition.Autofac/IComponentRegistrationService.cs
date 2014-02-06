namespace System.Composition.Autofac {
    public interface IComponentRegistrationService<TY> {
        void AttachToRegistration(IRegistrationContext context);
    }
}