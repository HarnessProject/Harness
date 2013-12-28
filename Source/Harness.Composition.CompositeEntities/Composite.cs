using System.Collections.Generic;

namespace Harness.Composition.CompositeEntities
{
    public class EntityAttributeDefinition {
            
    }

    public class Entity
    {
        public int Id { get; set; }

        public virtual ICollection<EntityAttributeDefinition> Attributes { get; set; } 
    }
}
