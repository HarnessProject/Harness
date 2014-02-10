using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Portable.Events;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public static class Roll {
        public static int D(int sides, int count = 1, bool reRoll1 = false, bool overroll = false) {
            var rolls = overroll ? count + 1 : count;
            var results = new int[rolls];

            var roll = 1;
            var resultNo = 0;
            var lowest = sides + 1;
            for (; roll <= rolls; roll++) {
                Roll:
                var r = new Random().Next(1, sides);
                if (reRoll1 && r == 1) goto Roll;
                if (r < lowest) lowest = r;
                results[resultNo++] = r;
            }
            return overroll ? results.TakeWhile(x => x != lowest).Sum() : results.Sum();
        }
    } 

    public class Stat : Reactive<KeyValuePair<string,dynamic>> {
        public Stat(string key, dynamic val) : base(new KeyValuePair<string, dynamic>(key,val)) {}

    }

    
    
}
