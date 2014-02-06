using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFxBugTest.ProofTest
{
    public static class Metrics
    {
        public static TimeSpan TimeAction(Action a) {
            var start = DateTime.Now;
            a();
            var stop = DateTime.Now;
            return stop.Subtract(start);
        }
    }
}
