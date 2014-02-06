using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Contracts
{
    public static class Metrics
    {
        /// <summary>
        /// The amount of time between the <param name="start">Start</param> and the specified <param name="stop">Stop</param>
        /// </summary>
        /// <param name="end">The end of the duration</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static TimeSpan Elapsed(this DateTime start, DateTime end)
        {
            return end.Subtract(start);
        }
        /// <summary>
        /// Returns the duration of an action.
        /// </summary>
        /// <param name="action">an <see cref="Action"/></param>
        /// <returns></returns>
        public static TimeSpan TimeAction(Action action)
        {
            var start = DateTime.Now;
            action();
            var stop = DateTime.Now;
            return  start.Elapsed(stop);
        }
    }
}
