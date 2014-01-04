using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Harness.Composition.CompositeEntities;

namespace Timesheets.Services.Models
{
    public class AttributeValue
    {
        [Key()]
        public int Id
        {
            get; set;
        }

        public string Value
        {
            get;
            set;
        }

        [Required]
        public virtual AttributeType AttributeType
        {
            get;
            set;
        }

        public int AttributeTypeId
        {
            get; set;
        }

        [NotMapped]
        public bool IsExternal { get { return ExternalDatabase.NotNull(); } }

        public virtual ExternalDatabase ExternalDatabase
        {
            get;
            set;
        }

        
        public int ExternalDatabaseId
        {
            get; set;
        }
    }
}
