using System;
using System.Collections.Generic;
using System.Linq;

namespace Harness.Services {
    public abstract class ServiceProvider<T> : IServiceProvider<T> {
        protected readonly ICollection<IRequirement> Requirements;
        protected readonly ICollection<Type> SupportedContexts;

        protected ServiceProvider() {
            Requirements = new HashSet<IRequirement>();
            SupportedContexts = new HashSet<Type>();
        }

        public ICollection<IRequirement> Required { get { return Requirements; } }

        public ICollection<Type> Supported { get { return SupportedContexts; } }

        public T Service { get; protected set; }
        public int Priority { get; protected set; }

        #region IServiceProvider<T> Members

        public EvaluationResult<T> Evaluate<TY>(TY context) {
            Type contextType = context.GetType();
            if (!SupportedContexts.Contains(contextType)) return new EvaluationResult<T> {IsMatch = false};
            List<IRequirement> contextualRequirements =
                Requirements
                    .Where(requirement => (requirement.Type.IsAssignableFrom(contextType)))
                    .ToList();
            return
                !contextualRequirements.Any() ||
                contextualRequirements
                    .All(requirement => requirement.Evaluate(context))
                    ? new EvaluationResult<T> {IsMatch = true, Priority = Priority, Service = Service}
                    : new EvaluationResult<T> {IsMatch = false};
        }

        #endregion

        public ServiceProvider<T> Requires<TY>(Func<TY, bool> requirement) {
            Requirements.Add(new Requirement<TY>(requirement));
            return this;
        }

        public ServiceProvider<T> Supports<TY>() {
            SupportedContexts.Add(typeof (TY));
            return this;
        }
    }
}