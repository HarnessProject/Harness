using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;

namespace System.Contracts
{
    public interface IToken : IDisposableDependency { }

    public interface IContinueToken : IToken {
        bool Continue { get; }
    }

    public interface ICancelToken : IToken {
        bool Canceled { get; }
    }
}
