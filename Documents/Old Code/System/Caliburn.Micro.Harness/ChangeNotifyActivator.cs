using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;

namespace Caliburn.Micro.Harness
{
    public class ChangeNotifyActivator : DependencyActivated<INotifyPropertyChangedEx>
    {
        public override void Activated(INotifyPropertyChangedEx component)
        {

            component.PropertyChanged += async (s, a) =>
                await s.AsTask(
                    v => component.Try(
                        z =>
                        {
                            Provider.Reflector.InvokeMemberAction(component, a.PropertyName + "Changed", v);
                            return true;
                        }
                    ).Act()
                );


        }
    }
}
