using Windows.ApplicationModel.Core;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;

namespace Harness.WinRT.CaliburnMicro {
    public class StateChangeModule : Module, IModule {
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration) {
            if (typeof (ISuspend).IsAssignableFrom(registration.Activator.LimitType))
                registration.Activated += ComponentSuspends;
            if (typeof (IResume).IsAssignableFrom(registration.Activator.LimitType))
                registration.Activated += ComponentActivates;
            if (typeof (IExit).IsAssignableFrom(registration.Activator.LimitType))
                registration.Activated += ComponentShutsdown;
        }

        private static void ComponentShutsdown(object sender, ActivatedEventArgs<object> activatedEventArgs) {
            var shutdown = activatedEventArgs.Instance as IExit;
            if (shutdown == null) return;
            CoreApplication.Exiting += (o, args) => shutdown.OnExit();
        }

        private static void ComponentActivates(object sender, ActivatedEventArgs<object> activatedEventArgs) {
            var activated = activatedEventArgs.Instance as IResume;
            if (activated == null) return;
            CoreApplication.Resuming += (o, args) => activated.OnResume();
        }

        private static void ComponentSuspends(object sender, ActivatedEventArgs<object> activatedEventArgs) {
            var suspended = activatedEventArgs.Instance as ISuspend;
            if (suspended == null) return;
            CoreApplication.Suspending +=
                (o, args) => suspended.OnSuspend(args.SuspendingOperation);
        }
    }
}