using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harness.Composition.CompositeEntities
{
    public class CompositeType
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class CompositePropertyValue {
        public int Id { get; set; }
        public int CompositeTypeId { get; set; }
        public virtual CompositeType CompositeType { get; set; } 
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Com

    
}
