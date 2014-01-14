namespace System.Contracts {
    public static class ContractExtensions {
        public static bool Assert<T>(this T o, Func<T, bool> assertion) {
            return assertion.Try(a => a(o)).Catch<Exception>((x,ex) => false).Invoke();
        }
    }
}