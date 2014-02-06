using System.Linq;

namespace System.Portable.Runtime {
    public class RegisteredSuppression {
        public Type OwnerType { get; set; }
        public Type SuppressionType { get; set; }

        public int ScoreSeed { get; set; }

        public int Score {
            get {
                var implmnts = SuppressionType.IsInterface && OwnerType.GetInterfaces().ToList().Contains(SuppressionType);
                var isImmediateDecendant = OwnerType.BaseType == SuppressionType;
                var isDecendant = OwnerType.Is(SuppressionType);

                var i = 0;
                if (isImmediateDecendant) i++; //a point for supressing your immediate base type
                if (implmnts) i++; //a point for supressing all implementations of an interface you implement
                if (isDecendant) i++; //a point for supressing a type in your lineage

                return i + ScoreSeed; //usually it's ordinal appearence when reflecting the appdomain.
            }
        }
    }
}