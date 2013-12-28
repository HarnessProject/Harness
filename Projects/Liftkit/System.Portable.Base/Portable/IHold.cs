using System.Runtime.CompilerServices;

namespace System.Portable {
    public interface IHold : IDisposable {
        IStrongBox Handle { get; set; }
        void ReleaseHandle();
    }
}