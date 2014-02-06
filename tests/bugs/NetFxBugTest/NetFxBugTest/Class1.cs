using System;
using System.Collections.Generic;
using System.Text;

namespace NetFxBugTest
{
    public interface IAmPclInterface { }

    public class PclClass : IAmPclInterface {
        public TY AsObject<TY>() {
            return (TY)(object)this;
        }

        public TY As<TY>() where TY : PclClass {
            return (TY) this;
        }
    }

    public class AnotherPclClass {
        public TY As<TY>() {
            return (TY)(object)this;
        }

        public static implicit operator AnotherPclClass(PclClass c) {
            return new AnotherPclClass();
        }

        public static implicit operator PclClass(AnotherPclClass c) {
            return new PclClass();
        }
    }
}
