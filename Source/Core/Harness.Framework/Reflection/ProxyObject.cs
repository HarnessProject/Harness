using Dynamitey;
using Dynamitey.DynamicObjects;
using Harness.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Harness.Framework.Reflection
{

    public class ProxyObject(object obj) : BaseForwarder(obj)
    {

    }

    public static class ReflectionExtensions
    {
        public static T ActLike<T>(this object target, params Type[] types)
        {
            var proxy = new Get(target);
            
    }
}
