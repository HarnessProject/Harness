namespace Harness.Services {
    public interface IServiceProvider<T> : IDependency {
        EvaluationResult<T> Evaluate<TY>(TY context);
    }
}