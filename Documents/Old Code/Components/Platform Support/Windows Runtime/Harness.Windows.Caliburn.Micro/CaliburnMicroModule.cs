﻿using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;
using Caliburn.Micro;
using Module = Autofac.Module;

namespace Harness.WinRT.CaliburnMicro {
    public class CaliburnMicroModule : Module, IModule {
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration) {
            if (typeof (INotifyPropertyChangedEx).IsAssignableFrom(registration.Activator.LimitType))
                registration.Activated += (o, args) =>
                    (args.Instance as INotifyPropertyChangedEx)
                        .PropertyChanged +=
                        async (s, a) =>
                            await Task.Factory.StartNew(
                                () => {
                                    TypeInfo type = s.GetType().GetTypeInfo();
                                    string handlerName = a.PropertyName + "Changed";
                                    MethodInfo handler = type.GetDeclaredMethod(handlerName);
                                    if (handler == null) return;
                                    if (handler.GetParameters().Length == 0)
                                        handler.Invoke(s, new object[] {});
                                }
                                );
        }
    }
}