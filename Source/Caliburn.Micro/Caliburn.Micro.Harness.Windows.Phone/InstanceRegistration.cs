#if NETFX_CORE
using Caliburn.Micro.Harness.Windows;
#endif
using Harness.Framework.Interfaces;

namespace Caliburn.Micro.Harness
{
    public class InstanceRegistration : IRegisterDependencies
    {
        public void Register(IDomainProvider typeProvider, IDependencyRegistrar registrar)
        {
            registrar.FactoryFor<IEventAggregator>(() => new EventAggregator()).AsSingleton();
#if NETFX_CORE
            registrar.FactoryFor<INavigationService>(() => Services.NavigationService).AsSingleton();
#endif
#if !NETFX_CORE && !SILVERLIGHT
            registrar.FactoryFor<IWindowManager>(() => new WindowManager()).AsSingleton();
#endif
        }

    }
}
