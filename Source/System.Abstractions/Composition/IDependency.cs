namespace System.Composition {
    public interface IDependency {} // All our IDependencys multi instance, per lifetime
    public interface ISingletonDependency {} //Except this one, single instance
    public interface ITransientDependency {} //And this, instance per dependency
    public interface IDisposableDependency : ITransientDependency, IDisposable{ } // Instance per dependency and disposable


   
}