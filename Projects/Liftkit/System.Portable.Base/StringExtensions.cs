using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System {
    public static class StringExtensions {
        public static bool IsMatch(this string str, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.IsMatch(str, pattern, options);
        }

        public static IEnumerable<Match> Matches(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            var r = new List<Match>();
            Regex.Matches(str, pattern, options).Each<Match>(r.Add);
            return r;
        } 
    }

    
}