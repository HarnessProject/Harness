using System;
using System.Collections.Generic;
using System.Linq;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Harness.Framework.Dependencies
{
    public class RegistrationContext : IRegistrationContext {
        

        private readonly Dictionary<Type, List<Action<IDependencyRegistration>>>
            _handlers = new Dictionary<Type, List<Action<IDependencyRegistration>>>();

        #region IRegistrationContext Members

        public void RegistrationHandlerForType<T>(Action<IDependencyRegistration> registration) {
            var t = typeof (T);
            if (!_handlers.ContainsKey(t)) 
                _handlers.Add(t, new List<Action<IDependencyRegistration>>());

            _handlers[t].Add(registration);
        }

        #endregion

        public IEnumerable<Action<IDependencyRegistration>> HandlersFor(Type t) {
            return from key in _handlers.Keys where key.Is(t) from h in _handlers[key] select h;
        }
    }
}