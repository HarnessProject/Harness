namespace System.Composition {
    public interface IDependency {} // All our IDependencys multi instance
    public interface ISingletonDependency {} //Except this one, single instance
    public interface ITransientDependency {}
    public interface IDisposableDependency : ITransientDependency, IDisposable{ }

   
}