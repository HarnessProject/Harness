namespace System.Composition {
    public class Token : IDisposableToken {
        
        public event Action OnDisposing;
        public bool Disposed { get; set; }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Dispose(bool closing) {
            if (closing) OnDisposing();
            Disposed = true;
            return true;
        }
    }
}