namespace System {
    public class SafeString {
        public string Value { get; set; }

        public static implicit operator string(SafeString s) {
            return s.Value;
        }

        public static implicit operator SafeString(string s) {
            return new SafeString {Value = s};
        }
    }
}