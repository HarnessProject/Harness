using System;

namespace Harness.Services {
    public interface IRequirement {
        Type Type { get; }
        bool Evaluate(object context);
    }
}