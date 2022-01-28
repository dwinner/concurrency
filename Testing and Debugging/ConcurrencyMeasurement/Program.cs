/**
 * Замеры параллелизма
 */

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConcurrencyMeasurement
{
   internal static class Program
   {
      private static void Main()
      {
         var rnd = new Random();
         var sourceData = new int[100000000];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = rnd.Next(0, int.MaxValue);
         }

         // Переменные для измерения
         const int numberOfIterations = 10;
         const int maxDegreeOfConcurrency = 16;

         var lockObj = new object();

         // Внешний цикл - степень параллелизма
         for (var concurrency = 1; concurrency <= maxDegreeOfConcurrency; concurrency++)
         {
            var stopWatch = Stopwatch.StartNew();
            var options = new ParallelOptions { MaxDegreeOfParallelism = concurrency };

            // Внутренний цикл повторяет итерации с той же степенью параллелизма
            for (var iteration = 0; iteration < numberOfIterations; iteration++)
            {
               double result = 0;
               Parallel.ForEach(sourceData, options, () => 0.0,
                  (value, loopState, index, localTotal) => localTotal + Math.Pow(value, 2), localTotal =>
                  {
                     lock (lockObj)
                     {
                        result += localTotal;
                     }
                  });
            }

            stopWatch.Stop();
            Console.WriteLine("Concurrency {0}: Per-iteration time is {1} ms", concurrency,
               stopWatch.ElapsedMilliseconds / numberOfIterations);
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}