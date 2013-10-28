namespace Harness.Services {
    public class EvaluationResult<T> {
        public bool IsMatch { get; set; }
        public int Priority { get; set; }
        public T Service { get; set; }
    }
}