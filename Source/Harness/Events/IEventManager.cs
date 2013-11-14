using System;

namespace Harness.Events {
    public interface IEventManager : ISingletonDependency {
        void Handle<T>(params Action<T>[] handlers) where T : class;
        void Trigger<T>(T eventObject) where T : class;
    }
}