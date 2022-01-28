using System;
using System.Collections.Generic;
using System.Linq;

namespace ParallelMapReduceSample
{
   public static class ParallelMapReduction
   {
      public static IEnumerable<TOutput> MapReduce<TInput, TIntermediate, TKey, TOutput>(
         IEnumerable<TInput> sourceData,
         Func<TInput, IEnumerable<TIntermediate>> mapFunction,
         Func<TIntermediate, TKey> groupFunction,
         Func<IGrouping<TKey, TIntermediate>, TOutput> reduceFunction)
      {
         return sourceData
            .AsParallel()
            .SelectMany(mapFunction)
            .GroupBy(groupFunction)
            .Select(reduceFunction);
      }
   }
}