/**
 * Аггрегирование контейнеров в параллельном режиме
 */

using System;
using System.Linq;

namespace ParallelReductionSample
{
   static class Program
   {
      static void Main()
      {
         // Простой и быстрый способ вычислить арифметическую прогрессию

         int[] sourceData = Enumerable.Range(0, 10).ToArray();
         Func<int, int, int> reduceFunction = (first, second) => first + second;
         Console.WriteLine("Result: {0}", ParallelReduce.Reduce(sourceData, 0, reduceFunction));
      }
   }
}
