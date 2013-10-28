namespace Harness {
    public interface IDependency {} // All our IDependencys per lifetime

    public interface ISingletonDependency : IDependency {} //Except this one, single instance

    public interface ITransientDependency : IDependency {} // And this one, per dependency
}