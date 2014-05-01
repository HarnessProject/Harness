#region ApacheLicense
// From the Harness Project
// Harness.Framework
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
#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;

#endregion

namespace Harness.Framework
{
    public interface IExtention { }

    internal class ExtentionContainer : IExtention { }

    public static class DeferedServices
    {
        public static Defered<IDependencyProvider> Dependencies { get; } =
            Defered<IDependencyProvider>.Create(
                () => Provider.Domain.FactoryFor<IDependencyProvider>().Create(),
                yieldOnce: true
            );

        public static Defered<IReflector> Reflector { get; } = Defered<IReflector>.Create(() => Provider.Domain.Create<IReflector>(), yieldOnce: true);

    }

    public static class Provider
    {
        public static IDictionary<string, object> Settings { get; } = new Dictionary<string,object>();

        public static IDomainProvider Domain { get; private set; }

        public static IDependencyProvider Dependencies { get { return DeferedServices.Dependencies.Result(); } }

        public static IReflector Reflector { get { return DeferedServices.Reflector.Result(); } }

        public static IDictionary<string, object> State { get; } = new Dictionary<string,object>();

        static Provider()
        {
            Domain = DomainProvider.Instance;
        }

        private static void Set<T>(Expression<Func<T>> property, T value)
        {
            property.Body.AsType<MemberExpression>().Member.AsType<PropertyInfo>().SetValue(null, value, null);
        }

        public static IExtention Get()
        {
            return new ExtentionContainer();
        }

        public static object Get(Type serviceType)
        {
            return Dependencies.Get(serviceType);
        }

        public static object Get(Type serviceType, string key)
        {
            return Dependencies.Get(serviceType, key);
        }

        public static IEnumerable<object> GetAll(Type serviceType)
        {
            return Dependencies.GetAll(serviceType);
        }

        public static T Get<T>()
        {
            return Dependencies.Get<T>();
        }

        public static T Get<T>(string key)
        {
            return Dependencies.Get<T>(key);
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return Dependencies.GetAll<T>();
        }
    }
}