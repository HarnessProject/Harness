using System.ComponentModel.DataAnnotations.Schema;

namespace Harness.Composition.CompositeEntities {
    public class ExternalDatabase
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public string ProviderType
        {
            get;
            set;
        }

        [NotMapped()]
        public IDataRepository Repository
        {
            get;
            set;
        }
    }
}