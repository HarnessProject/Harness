namespace System.Portable.Events {
    public class RegisteredFilter {
        public DelegatePipeline Pipeline { get; set; }
        public Guid Id { get; set; }
        public Type TargetType { get; set; }
        public DelegateFilter Filter { get; set; }
    }
}