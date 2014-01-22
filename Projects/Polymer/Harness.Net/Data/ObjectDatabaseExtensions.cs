using System.Portable.Runtime;
using Lucene.Net.Store;

namespace System.Data {
    public static class ObjectDatabaseExtensions
    {
        public const string StateKey = "Global.Database";
        public static ObjectDatabase Database(this IScope scope, Directory dir = null)  {            
            if (!scope.State.ContainsKey(StateKey)) scope.State.Add(StateKey, new ObjectDatabase(dir ?? new RAMDirectory()));
            return scope.State[StateKey].As<ObjectDatabase>();
        }


        
        
    }
}