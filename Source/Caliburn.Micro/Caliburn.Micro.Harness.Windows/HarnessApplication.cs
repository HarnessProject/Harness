using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Caliburn.Micro.Harness.Windows
{
    public class HarnessApplication : CaliburnApplication
    {
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return base.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return base.GetInstance(service, key);
        }

        protected override void BuildUp(object instance)
        {
            base.BuildUp(instance);
        }

        protected override void Configure()
        {
            base.Configure();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            Services.NavigationService = new FrameAdapter(rootFrame);
        }
       


    }
}
