using System;
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
                x => registration.Activated += (o, args) =>
                    (args.Instance.As<INotifyPropertyChangedEx>())
                    .PropertyChanged +=
                    async (s, a) =>
                    await Task.Factory.StartNew(
                        () => {
                            Type type = s.GetType();
                            string handlerName = a.PropertyName + "Changed";
                            MethodInfo handler = type.GetMethod(handlerName);
                            if (handler == null) return;
                            if (handler.GetParameters().Length == 0) handler.Invoke(s, new object[] {});
                        }
                    )
                );

    }
}