using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;

namespace System.Portable.Runtime.Dynamic
{
    public class DynamicInvoker : IDynamicInvoker
    {
        public object InvokeReturn(Delegate del, params dynamic[] args) {
            return del.FastDynamicInvoke((object[])args);
        }

        public void InvokeAction(Delegate del, params dynamic[] args) {
            del.FastDynamicInvoke(args);
        }
    }
}
