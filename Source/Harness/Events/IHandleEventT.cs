using System;

namespace Harness.Events {
    public interface IHandleEvent<in T> : IDependency {
        Type EventType { get; }
        void Handle(T eventObject);
    }
}