using System;
using System.Collections.Generic;
using System.Linq;
using Harness.Framework.Collections;

namespace Harness.Framework.Net
{
    public delegate void ModelDefinition<T, TKey>(
            Func<T> newModel = null,
            Func<TKey> defaultKey = null,
            Func<TKey, TKey, bool> keysEqual = null,
            Func<IEnumerable<T>> emptyCollection = null,
            Func<IEnumerable<T>> all = null,
            Func<IQueryable<T>> asQueryable = null);

    public class LogEvent(
        string source = "Null",
        string target = "Null",
        string message = "A logable event has occurred",
        DateTime? timestamp = null,
        Guid parent = default(Guid),
        Guid id = default(Guid)
    ) : Model<LogEvent, Guid>(
        id: id,
        parent: parent
    )
    {
        public string ExceptionName { get; set; } = string.Empty;

        public string Message { get; set; } = message;

        public string Source { get; set; } = source;

        public string Target { get; set; } = target;

        public DateTime Timestamp { get; set; } = timestamp.HasValue ? timestamp.Value : DateTime.Now;

        public static void Define(ModelDefinition<LogEvent, Guid> defineModel)
        {
            defineModel(
                keysEqual: (k1, k2) => k1 == k2,
                defaultKey: () => Guid.NewGuid(),
                newModel: () => new LogEvent()
            );
        }

        public virtual IEnumerable<LogEvent> Ancestors()
        {
            return EmptyCollection();
        }

        public virtual IEnumerable<LogEvent> Children()
        {
            return EmptyCollection();
        }
    }

    public class Model<T, TKey>(
             TKey id = default(TKey),
         TKey parent = default(TKey)
     )
    {
        public static Func<IEnumerable<T>> All { get; private set; } = EmptyArray;
        public static Func<IQueryable<T>> AsQueryable { get; private set; } = EmptyArray().AsQueryable;
        public static Func<TKey> DefaultKey { get; private set; } = () => default(TKey);
        public static Func<IEnumerable<T>> EmptyCollection { get; private set; } = EmptyArray;
        public static Func<TKey, TKey, bool> KeysEqual { get; private set; } = (k1, k2) => EqualityComparer<TKey>.Default.Equals(k1, k2);

        public static Func<T> New { get; private set; } = () => default(T);

        public TKey Id { get; } = KeysEqual(id, default(TKey)) ? DefaultKey() : id;

        public TKey Parent { get; } = KeysEqual(parent, default(TKey)) ? default(TKey) : parent;

        public static void DefineModel(
            Func<T> newModel = null,
            Func<TKey> defaultKey = null,
            Func<TKey, TKey, bool> keysEqual = null,
            Func<IEnumerable<T>> emptyCollection = null,
            Func<IEnumerable<T>> all = null,
            Func<IQueryable<T>> asQueryable = null) {

            New = newModel ?? New;
            DefaultKey = defaultKey ?? DefaultKey;
            KeysEqual = keysEqual ?? KeysEqual;
            EmptyCollection = emptyCollection ?? EmptyCollection;
            All = all ?? All;
            AsQueryable = asQueryable ?? AsQueryable;
        }

        protected static T[] Array(params T[] items)
        {
            return items;
        }

        protected static IEnumerable<T> EmptyArray()
        {
            return new T[] { };
        }
    }
}