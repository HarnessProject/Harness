namespace Harness.Core {
    public class Application<T> : IApplication
        where T : IEnvironment {
        protected Application(T environment, bool isGlobal) {
            Environment = environment;
            if (isGlobal) SetGlobalApplication();
        }

        #region IApplication Members

        public IEnvironment Environment { get; protected set; }

        #endregion

        public static Application<T> New(T environment, bool isGlobal = true) {
            return new Application<T>(environment, isGlobal);
        }

        protected void SetGlobalApplication() {
            Application.CurrentEnvironment = Environment;
            Application.CurrentApplication = this;
        }
    }
}