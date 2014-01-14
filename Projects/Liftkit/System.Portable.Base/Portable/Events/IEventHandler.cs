using System.Collections.Generic;
using System.Portable.Runtime;
using System.Runtime.CompilerServices;

namespace System.Portable.Events {
    public interface IEventHandler : IDisposable  {
        IList<Guid> Registrations  { get; set; } 
        void Register(IEventManager em);
    }
}