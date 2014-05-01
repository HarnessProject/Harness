#region ApacheLicense
// From the Harness Project
// Caliburn.Micro.Harness
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
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
                .Where(type => type.Is<INotifyPropertyChanged>());
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

            //  register the single event aggregator for this container

            typeProvider.Types.Where(t => t.Is<IShell>()).Each(t => registrar.Register(t).As<IShell>().AsSelf().AsTransient());

        }
    }
}
