using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Portable.IO;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Abstractions;
using Remotion.Linq.Parsing.Structure;
using Directory = Lucene.Net.Store.Directory;

namespace System.Data
{
    public class ObjectDatabase : LuceneDataProvider, IDataProvider
    {
        public ObjectDatabase(Directory dir) : base(dir, Lucene.Net.Util.Version.LUCENE_30) {}

        public ISession<T> Session<T>() where T : new() {
            return OpenSession<T>().ActLike<ISession<T>>(); // Impomptu interface saves us from a wrapper.
        }

        public IQueryable<T> Query<T>() where T : new() {
            return AsQueryable<T>();
        }
    }
}
