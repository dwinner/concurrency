using System;
using System.Linq;

namespace ParallelReductionSample
{
   public static class ParallelReduce
   {
      public static TValue Reduce<TValue>(TValue[] sourceData, TValue seedValue,
         Func<TValue, TValue, TValue> reduceFunction)
      {
         return sourceData
            .AsParallel()
            .Aggregate(seedValue, reduceFunction, reduceFunction, overallResult => overallResult);
      }
   }
}