#region ApacheLicense
// From the Harness Project
// Caliburn.Micro.Harness.Win
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
using Harness.Framework;
using Harness.Framework.Collections;
using Harness.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Windows.UI.Xaml.Controls;

namespace Caliburn.Micro.Harness { 
    public class HarnessApplication : CaliburnApplication
    {
        
        }



    

    public class WinDependencyRegistrar : IDependencyRegistrar
    {
        protected TinyIoCContainer Container { get; } = new TinyIoCContainer();
        public IDependencyRegistration FactoryFor<T>(Func<T> creator)
        {
            return new WinDependencyRegistration(typeof(T), Container, Container.Register(typeof(T), (c, p) => creator()));
        }
        public IDependencyRegistration Register(Type type)
        {
            return new WinDependencyRegistration(type, Container, Container.Register(type));
        }

        public IDependencyRegistration Register<T>()
        {
            return new WinDependencyRegistration(typeof(T), Container, Container.Register(typeof(T)));
        }
    }

    public class WinDependencyRegistration(Type type, TinyIoCContainer container, TinyIoCContainer.RegisterOptions options) : IDependencyRegistration
    {
        public Type Type { get; } = type;
        public IList<TinyIoCContainer.RegisterOptions> Options { get; } = new List<TinyIoCContainer.RegisterOptions> { options };
        public TinyIoCContainer Container { get; } = container;

        public IDependencyRegistration As(Type type)
        {
            Options.Add(Container.Register(type, Type));
            return this;
        }

        public IDependencyRegistration As<T>()
        {
            Options.Add(Container.Register(typeof(T), Type));
            return this;
        }

        public IDependencyRegistration AsAncestors()
        {
            Provider.Domain.GetAncestorsOf(Type).Each(t => As(t));
            return this;
        }

        public IDependencyRegistration AsAny()
        {
            return AsSelf().AsAncestors().AsImplemented();
        }

        public IDependencyRegistration AsImplemented()
        {
            Type.GetTypeInfo().ImplementedInterfaces.Select(t => Container.Register(t, Type)).AddTo(Options);
            return this;
        }

        public IDependencyRegistration AsSelf()
        {
            Options.Add(Container.Register(Type));
            return this;
        }

        public IDependencyRegistration AsSingleInScope()
        {
            Options.Each(o => o.AsSingleton());
            return this;
        }

        public IDependencyRegistration AsSingleton()
        {
            Options.Each(o => o.AsSingleton());
            return this;
        }

        public IDependencyRegistration AsTransient()
        {
            Options.Each(o => o.AsMultiInstance());
            return this;
        }

        public IDependencyRegistration InjectProperties(bool preserveValues)
        {
            return this;
        }

        public IDependencyRegistration RegisterAsEach(IEnumerable<Type> types)
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration RegisterAsEach(params Type[] types)
        {
            throw new NotImplementedException();
        }
    }
}
