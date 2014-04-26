using System;
using Harness.Framework.Extensions;

namespace Harness.Framework.Interfaces
{
    public interface IDependencyActivated : IDependency
    {
        Type ForType { get; }
        void Activated(object component);
    }

    public abstract class DependencyActivated<T> : IDependencyActivated
    {
        public Type ForType { get; } = typeof(T);

        public void Activated(object component)
        {
            Activated(component.AsType<T>());
        }

        public abstract void Activated(T component);
    }
}
