using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;
using Caliburn.Micro;
using Harness.Framework;
using Module = Autofac.Module;

namespace Harness.Net.Caliburn.Micro{
    public class CaliburnMicroModule : Module, IModule {
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration) {

            typeof (INotifyPropertyChangedEx).If(
                registration.Activator.LimitType.CanBe,
                x => registration.Activated +=
                    async (o, args) =>
                    await args
                    .Instance
                    .As<INotifyPropertyChangedEx>()
                    .Try<INotifyPropertyChangedEx, bool>(
                        y => {
                            y.PropertyChanged += async (s, a) => await s.ResolveAndInvokeAsync(a.PropertyName + "Changed");
                            return true;
                        }
                    )
                    .Catch<Exception>((y, ex) => false)
                    .InvokeAsync()
                    .Begin()
                );
        }
    }
}