using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Contracts
{
    public static class Metrics
    {
        public static TimeSpan Elapsed(DateTime start, DateTime stop)
        {
            return stop.Subtract(start);
        }
        public static TimeSpan TimeAction(Action action)
        {
            var start = DateTime.Now;
            action();
            var stop = DateTime.Now;
            return Elapsed(start, stop);
        }
    }
}
