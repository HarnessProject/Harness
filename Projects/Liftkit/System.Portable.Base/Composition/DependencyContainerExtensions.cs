using System;
using System.Collections.Generic;
using System.Linq;
using System.Portable.Runtime;
using System.Text;

namespace System.Composition
{
    public static class DependencyContainerExtensions
    {
        public static bool ExecuteIf<T>(this IDependencyProvider conatiner, Action<T> action) where T : IDependencyProvider {
            if (!(conatiner is T)) return false;
            action(conatiner.As<T>());
            return true;
        }
        
        
    }
}
