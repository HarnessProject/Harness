using System;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;

namespace Caliburn.Micro.Harness
{
    

    public class EventSubscriptionActivator : DependencyActivated<IHandle>
    {
        public override void Activated(IHandle handle)
        {
            Provider.Get<IEventAggregator>().Subscribe(handle);
        }
    }
}