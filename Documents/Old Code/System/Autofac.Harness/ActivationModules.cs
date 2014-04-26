using Autofac;
using Harness.Framework;
using Harness.Framework.Interfaces;
using System.Linq;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Autofac.Core;

namespace Autofac.Harness
{
    public class ActivationModule : Module, IModule
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Activated += Registration_Activated;
        }

        private static void Registration_Activated(object sender, ActivatedEventArgs<object> e)
        {
            if (
                e.Instance.Is<IDependencyActivated>() || 
                e.Instance.Is<IDependencyActivated[]>()
            ) return;

            Provider
            .GetAll<IDependencyActivated>()
            .Where(x => x.ForType.Is(e.Instance.GetType()))
            .Each(x => x.Activated(e.Instance));
        }
    }

    

    
}