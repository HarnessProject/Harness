namespace System.Portable.IO {
    public class Path {
        public static char[] KnownSeperators {
            get {
                return new[] {'/', '\\'};
            }
        }

        /// <summary>
        /// Gets the Regex Pattern of known roots for paths...
        /// </summary>
        /// <value>
        /// Patterns for known path roots.
        /// </value>
        public static string[] KnownRoots {
            get {
                return new[] {@"[\w]\:[.*]", @"\/[.*]", @"[\w*]\:[.*]"};
            }
        }
    }
}