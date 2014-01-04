using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Portable.Events {
    public interface IEventHandler : IDisposable  {
        IDictionary<Guid, DelegatePipeline> Registrations { get; set; } 
        bool ShouldRegister(DelegatePipeline pipeline);
        void Register(DelegatePipeline pipeline);
    }
}