using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;

namespace System.Composition.CaliburnMicro.Autofac{
    public class CaliburnMicroModule : Module, Composition.Autofac.IModule {
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration) {

            typeof (INotifyPropertyChangedEx)
                .If(registration.Activator.LimitType.Is, 
                x => registration.Activated +=
                    async (o, args) =>
                    await args
                    .Instance
                    .As<INotifyPropertyChangedEx>()
                    .Try<INotifyPropertyChangedEx, bool>(
                        y => {
                            y.PropertyChanged += 
                                (s, a) => 
                                    s.GetType()
                                    .GetMethod(a.PropertyName + "Changed")
                                    .NotNull(z => z.Invoke(s, null));
                            return true;
                        }
                    )
                    .Catch<Exception>((y, ex) => false)
                    .AsTask(y => y.Act()));
        }
    }
}