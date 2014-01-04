using System;
using System.Collections.Generic;
using System.Composition;
using System.Data.Entity;
using System.Events;
using System.Linq;
using System.Runtime.Environment;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Logging.NLog;
using Harness.Autofac;
using Harness.TinyMessenger;
using Quartz.Core;
using Timesheets.Services.Models;

namespace Timesheets.Services
{
    

    public class Application : IRegistrationProvider<ContainerBuilder> {
        
        public static class Timesheets {
            public static async Task Start(bool serviceMode = false) {
                await Application.InitializeAsync(
                    x => {
                        x.State["ServiceMode"] = serviceMode;
                        x.Container = new AutofacDependencyContainer(new TypeProvider());
                        x.MessengerHub = new TinyMessengerHub();
                        x.EventMessenger = new EventMessenger(x.MessengerHub);
                    }
                );

                await Application.Global.EventMessenger.Trigger(new ApplicationStartEvent(null, Application.Global));
                
            }
        }

        public void Register(ITypeProvider typeProvider, ContainerBuilder builder) {
            builder.RegisterType<NLogLogger>().AsSelf().AsImplementedInterfaces().SingleInstance();
            
            if (Application.Global.State["ServiceMode"].As<bool>()) 
                builder.RegisterType<QuartzScheduler>().AsSelf().AsImplementedInterfaces().SingleInstance();
            
            builder.Register(c => new ModelsContext());
        }
    }
}
