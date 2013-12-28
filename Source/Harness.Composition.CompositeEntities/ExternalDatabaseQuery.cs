namespace Harness.Composition.CompositeEntities {
    public class ExternalDatabaseQuery
    {
        public int ExternalDatabaseId
        {
            get;
            set;
        }

        public ExternalDatabase ExternalDatabase
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public string QueryLanguage {
            get;
            set;
        }
    }
}