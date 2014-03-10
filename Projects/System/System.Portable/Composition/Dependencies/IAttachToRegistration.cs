

namespace System.Composition.Dependencies {
    public interface IAttachToRegistration<TY> {
        void AttachToRegistration(IRegistrationContext context);
    }
}