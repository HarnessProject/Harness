﻿using System.Composition;
using System.Threading.Tasks;

namespace System.Contracts
{
    public interface IToken : IDisposableDependency { }

    public interface IContinueToken : IToken {
        Task Task { get; }
        bool Continue { get; }
        
    }

    public interface ICancelToken : IToken {
        Task Task { get; }
        bool Canceled { get; }
    }
}
