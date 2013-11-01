using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Harness.Framework;

namespace Harness {
    public class RegistrationContext : IRegistrationContext {
        private readonly List<Action<ContainerBuilder>> _builders = new List<Action<ContainerBuilder>>();

        private readonly Dictionary<Type, List<Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>>>
            _handlers = new Dictionary<Type, List<Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>>>();

        #region IRegistrationContext Members

        public void RegisterHandlerForType<T>(Action<IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registration) {
            var t = typeof (T);
            if (!_handlers.ContainsKey(t)) 
                _handlers.Add(t, new List<Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>>());

            _handlers[t].Add(registration.As<Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>>());
        }

        public void RegisterContainerBuilder(ContainerBuilder builder, Action<ContainerBuilder> newBuilder) {
            _builders.Add(newBuilder);
        }

        #endregion

        public IEnumerable<Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>> HandlersFor(Type t) {
            return from key in _handlers.Keys where key.IsAssignableFrom(t) from h in _handlers[key] select h;
        }
    }
}