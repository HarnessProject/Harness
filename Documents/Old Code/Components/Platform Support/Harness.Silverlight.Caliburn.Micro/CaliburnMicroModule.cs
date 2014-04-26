//using System.Threading.Tasks;

//using System.Threading.Tasks;
using Caliburn.Micro;

namespace Harness.Silverlight.CaliburnMicro
{
    public class CaliburnMicroModule : Autofac.Module, Harness.IModule
    {
        protected override void AttachToComponentRegistration(Autofac.Core.IComponentRegistry componentRegistry, Autofac.Core.IComponentRegistration registration)
        {
            if (typeof(INotifyPropertyChangedEx).IsAssignableFrom(registration.Activator.LimitType))
                registration.Activated += (o, args) =>
                                          (args.Instance as INotifyPropertyChangedEx)
                                              .PropertyChanged +=
                                                    (s, a) =>
                                                    {
                                                        var type = s.GetType();
                                                        var handlerName = a.PropertyName + "Changed";
                                                        var handler = type.GetMethod(handlerName);
                                                        if (handler == null) return;
                                                        if (handler.GetParameters().Length == 0)
                                                            handler.Invoke(s, new object[] {});
                                                    };
                                    
        }
    }
}
