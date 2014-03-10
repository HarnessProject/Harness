#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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

using System.Collections.Generic;

#endregion

namespace System.Composition.Providers {
    //
    // 
    // 

    /// <summary>
    ///     An implementation of the Common Service Locator Pattern.
    ///     Implements IServiceProvider.
    /// </summary>
    public interface IDependencyProvider : IDisposable, IServiceProvider {
        object Get(Type serviceType);
        object Get(Type serviceType, string key);
        IEnumerable<object> GetAll(Type serviceType);
        T Get<T>();
        T Get<T>(string key);
        IEnumerable<T> GetAll<T>();
    }
}

#region CSLP

/*
// Portable IServiceLocator for recompiling components expecting the Microsoft ServiceLocator
namespace Microsoft.Practices.ServiceLocation {
    public interface IServiceLocator : IServiceProvider {
        object GetInstance(Type serviceType);
        object GetInstance(Type serviceType, string key);
        IEnumerable<object> GetAllInstances(Type serviceType);
        TService GetInstance<TService>();
        TService GetInstance<TService>(string key);
        IEnumerable<TService> GetAllInstances<TService>();
    }

    public delegate IServiceLocator ServiceLocatorProvider();

    public class ActivationException : Exception {
        public ActivationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class ServiceLocator {
        private static IServiceLocator _locator;
        private static ServiceLocatorProvider _provider;
        public static void SetLocatorProvider(ServiceLocatorProvider newProvider) {
            _provider = newProvider;
        }

        public static IServiceLocator Current {
            get {
                if (_provider == null) return null;
                return _locator ?? (_locator = _provider());
            }
        }
    }

    /// <summary>
    /// This class is a helper that provides a default implementation
    /// for most of the methods of <see cref="IServiceLocator"/>.
    /// </summary>
    public abstract class ServiceLocatorImplBase : IServiceLocator
    {
        /// <summary>
        /// Implementation of <see cref="IServiceProvider.GetService"/>.
        /// </summary>
        /// <param name="serviceType">The requested service.</param>
        /// <exception cref="ActivationException">if there is an error in resolving the service instance.</exception>
        /// <returns>The requested object.</returns>
        public virtual object GetService(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        /// <summary>
        /// Get an instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <exception cref="ActivationException">if there is an error resolving
        /// the service instance.</exception>
        /// <returns>The requested service instance.</returns>
        public virtual object GetInstance(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        /// <summary>
        /// Get an instance of the given named <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <param name="key">Name the object was registered with.</param>
        /// <exception cref="ActivationException">if there is an error resolving
        /// the service instance.</exception>
        /// <returns>The requested service instance.</returns>
        public virtual object GetInstance(Type serviceType, string key)
        {
            try
            {
                return DoGetInstance(serviceType, key);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivationExceptionMessage(ex, serviceType, key),
                    ex);
            }
        }

        /// <summary>
        /// Get all instances of the given <paramref name="serviceType"/> currently
        /// registered in the container.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <exception cref="ActivationException">if there is are errors resolving
        /// the service instance.</exception>
        /// <returns>A sequence of instances of the requested <paramref name="serviceType"/>.</returns>
        public virtual IEnumerable<object> GetAllInstances(Type serviceType)
        {
            try
            {
                return DoGetAllInstances(serviceType);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivateAllExceptionMessage(ex, serviceType),
                    ex);
            }
        }

        /// <summary>
        /// Get an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <exception cref="ActivationException">if there is are errors resolving
        /// the service instance.</exception>
        /// <returns>The requested service instance.</returns>
        public virtual TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService), null);
        }

        /// <summary>
        /// Get an instance of the given named <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <param name="key">Name the object was registered with.</param>
        /// <exception cref="ActivationException">if there is are errors resolving
        /// the service instance.</exception>
        /// <returns>The requested service instance.</returns>
        public virtual TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        /// <summary>
        /// Get all instances of the given <typeparamref name="TService"/> currently
        /// registered in the container.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <exception cref="ActivationException">if there is are errors resolving
        /// the service instance.</exception>
        /// <returns>A sequence of instances of the requested <typeparamref name="TService"/>.</returns>
        public virtual IEnumerable<TService> GetAllInstances<TService>() {
            return GetAllInstances(typeof(TService)).Select(item => (TService)item);
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        /// the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected abstract object DoGetInstance(Type serviceType, string key);

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        /// resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving a single service.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <param name="key">Name requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivationExceptionMessage(Exception actualException, Type serviceType, string key)
        {
            return string.Format(CultureInfo.CurrentUICulture, "An activation exception occurred solving an instance, Type: {0}, Key {1} ", serviceType.Name, key);
        }

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving multiple service instances.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivateAllExceptionMessage(Exception actualException, Type serviceType)
        {
            return string.Format(CultureInfo.CurrentUICulture, "An activation exception occurred resolving multiple instances, Type: {0}, Key {1} ", serviceType.Name);
        }
    }
}

*/

#endregion