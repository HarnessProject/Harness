namespace Harness {
    public interface IDemand<T> : IRequest<T> {
      
        // Avoid Asyncing a demand...
    }

    public interface IRequest<T> {
        
    }
}