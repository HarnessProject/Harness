namespace System.Portable.Events {
    public class RegisteredAction {
        public DelegatePipeline Pipeline { get; set; }
        public Guid Id { get; set; }
        public Type TargetType { get; set; }
        public DelegateAction Handler { get; set; }
        public DelegateFilter Filter { get; set; }
    }
}