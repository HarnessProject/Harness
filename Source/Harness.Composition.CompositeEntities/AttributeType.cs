using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Timesheets.Services.Models;

namespace Harness.Composition.CompositeEntities
{
    public class AttributeType
    {
        [Key()]
        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public virtual ICollection<AttributeValue> Values { get; set; }

        public bool AllowCustomValues
        {
            get; set;
        } 
    }
}
