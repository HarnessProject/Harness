using System;
using System.Collections.Generic;
using System.Reflection;
using System.Tasks;
using System.Threading.Tasks;
using Autofac.Core;
using Caliburn.Micro;
using IModule = Harness.Autofac.IModule;
using Module = Autofac.Module;

namespace Harness.Net.Caliburn.Micro{
    public class CaliburnMicroModule : Module, IModule {
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration) {

            typeof (INotifyPropertyChangedEx).If(
                registration.Activator.LimitType.Is,
                x => registration.Activated +=
                    async (o, args) =>
                    await args
                    .Instance
                    .As<INotifyPropertyChangedEx>()
                    .Try<INotifyPropertyChangedEx, bool>(
                        y => {
                            y.PropertyChanged += (s, a) => s.GetType().GetMethod(a.PropertyName + "Changed").NotNull(z => z.Invoke(s, null));
                            return true;
                        }
                    )
                    .Catch<Exception>((y, ex) => false)
                    .AsTask(y => y.Invoke())
                    
                );
        }
    }
}