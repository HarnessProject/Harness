using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Caliburn.Micro.Harness
{
    public class CaliburnMicroRegistration : IRegisterDependencies
    {
        public void Register(IDomainProvider typeProvider, IDependencyRegistrar registrar)
        {

            //  register viewmodels
            var viewModels = typeProvider.Types
                //  must be a type that ends with ViewModel
                .Where(type => type.Name.EndsWith("ViewModel"))
                //  must be in a namespace ending with ViewModels
                .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("ViewModels"))
                //  must implement INotifyPropertyChanged (deriving from PropertyChangedBase will statisfy this)
                .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name) != null);
            //  registered as self and always create a new one
            viewModels.Each(x => registrar.Register(x).AsSelf().AsTransient());


            //  register views
            var views = typeProvider.Types
                //  must be a type that ends with View
                .Where(type => type.Name.EndsWith("View"))
                //  must be in a namespace that ends in Views
                .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("Views"));
            //  registered as self and always create a new one
            views.Each(x => registrar.Register(x).AsSelf().AsTransient());


            //  register the single window manager for this container
            registrar.FactoryFor<IWindowManager>(() => new WindowManager()).AsSingleton();
            //  register the single event aggregator for this container
            registrar.FactoryFor<IEventAggregator>(() => new EventAggregator()).AsSingleton();

            typeProvider.Types.Where(t => t.Is<IShell>()).Each(t => registrar.Register(t).As<IShell>().AsSelf().AsTransient());

        }
    }
}
