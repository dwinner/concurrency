using System;
using System.Collections.Concurrent;

namespace ParallelCache
{
   public class ParallelCache<TKey, TValue>
   {
      private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary;
      private readonly Func<TKey, TValue> _factory;

      public ParallelCache(Func<TKey, TValue> factory)
      {
         _factory = factory;
         _dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
      }

      public TValue GetValue(TKey key)
      {
         return _dictionary.GetOrAdd(key, new Lazy<TValue>(() => _factory(key))).Value;
      }
   }
}