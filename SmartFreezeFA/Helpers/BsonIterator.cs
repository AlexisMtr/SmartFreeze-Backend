using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace SmartFreezeFA.Helpers
{
    public static class BsonIterator
    {
        public static TResult Iterate<TSource, TResult>(IMongoCollection<TSource> collection, PipelineDefinition<TSource, BsonDocument> pipeline, Func<BsonDocument, TResult, TResult> callback)
        {
            var docCursor = collection.Aggregate(pipeline);

            TResult value = default(TResult);
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    value = callback.Invoke(item, value);
                }
            }
            return value;
        }

        public static TResult Iterate<TSource, TResult>(IMongoCollection<TSource> collection, PipelineDefinition<TSource, BsonDocument> pipeline, Func<string, TResult, TResult> callback)
        {
            var docCursor = collection.Aggregate(pipeline);

            TResult value = default(TResult);
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    value = callback.Invoke(item.ToJson(), value);
                }
            }
            return value;
        }
    }
}
