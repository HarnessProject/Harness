﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Portable.Runtime;

using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Harness.Autofac
{
    [SuppressDependency(typeof(NullDependencyProvider))]
    public class AutofacDependencyProvider : IDependencyProvider {
        public IContainer Container { get; set; }

        public AutofacDependencyProvider(ITypeProvider environment) {
            Container = new AutofacContainerFactory(environment).Create();
        }

        public AutofacDependencyProvider(IContainer container) {
            Container = container;
        }

        public void Dispose() {
            Container.Dispose();
        }

        public object GetService(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object Get(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object Get(Type serviceType, string key) {
            return Container.ResolveNamed(key, serviceType);
        }

        public IEnumerable<object> GetAll(Type serviceType) {
            return ((IEnumerable)Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType))).Cast<Object>();
        }

        public TService Get<TService>() {
            return Container.Resolve<TService>();
        }

        public TService Get<TService>(string key) {
            return Container.ResolveNamed<TService>(key);
        }

        public IEnumerable<TService> GetAll<TService>() {
            return Container.Resolve<IEnumerable<TService>>();
        }

        
    }
}
