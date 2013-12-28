using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Composition
{
    public static class DependencyContainerExtensions
    {
        public static bool ExecuteIf<T>(this IDependencyContainer conatiner, Action<T> action) where T : IDependencyContainer {
            if (!(conatiner is T)) return false;
            action(conatiner.As<T>());
            return true;
        }
        
        
    }
}
