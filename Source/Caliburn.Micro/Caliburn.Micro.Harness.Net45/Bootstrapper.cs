#region ApacheLicense
// From the Harness Project
// Caliburn.Micro.Harness.SL5
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
using System.Reflection;
using System.Windows;
using Harness.Framework;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Caliburn.Micro.Harness
{

    public class Bootstrapper : BootstrapperBase, IDependency {
        public Bootstrapper(bool useApplication = true) : base(useApplication)
        {
            StartRuntime();
        }
        
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return Provider.Domain.Assemblies;
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = string.IsNullOrWhiteSpace(key) ?
                Provider.Get(service) :
                Provider.Get(service, key);
            if (instance.NotDefault()) return instance;
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Provider.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            Provider.Dependencies.InjectProperties(instance);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}