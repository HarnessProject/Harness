#region ApacheLicense
// From the Harness Project
// Autofac.Harness
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
using Autofac;
using Harness.Framework;
using Harness.Framework.Interfaces;
using System.Linq;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Autofac.Core;

namespace Autofac.Harness
{
    public class ActivationModule : Module, IModule
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Activated += Registration_Activated;
        }

        private static void Registration_Activated(object sender, ActivatedEventArgs<object> e)
        {
            if (
                e.Instance.Is<IDependencyActivated>() || 
                e.Instance.Is<IDependencyActivated[]>()
            ) return;

            Provider
            .GetAll<IDependencyActivated>()
            .Where(x => x.ForType.Is(e.Instance.GetType()))
            .Each(x => x.Activated(e.Instance));
        }
    }

    

    
}