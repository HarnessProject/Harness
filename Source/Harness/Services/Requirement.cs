using System;

namespace Harness.Services {
    public class Requirement<T> : IRequirement {
        public Requirement(Func<T, bool> evaluator) {
            Evaluator = evaluator;
            Type = typeof (T);
        }

        public Func<T, bool> Evaluator { get; set; }

        #region IRequirement Members

        public Type Type { get; set; }

        public bool Evaluate(object context) {
            try {
                return Evaluator((T) context);
            }

            catch {
                return false;
            }
        }

        #endregion
    }
}