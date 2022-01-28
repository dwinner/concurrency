/**
 * Параллельная обработка данных
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TPLPrimes.Extensions;

namespace TPLPrimes
{
   public static class Program
   {
      private const int MAX_PRIMES = 1000000;
      private const int MAX_NUMBER = 20000000;

      private static void Main()
      {
         long primesFound = 0;
         Console.WriteLine("Iterative");
         Stopwatch watch = new Stopwatch();
         watch.Start();
         for (uint i = 0; i < MAX_NUMBER; i++)
         {
            if (!i.IsPrime())
               continue;
            Interlocked.Increment(ref primesFound);
            if (primesFound <= MAX_PRIMES)
               continue;
            Console.WriteLine("Last prime found: {0:N0}", i);
            break;
         }
         watch.Stop();
         Console.WriteLine("Found {0:N0} primes in {1}",
            primesFound, watch.Elapsed);
         watch.Reset();

         primesFound = 0;
         Console.WriteLine("Parallel");
         watch.Start();

         // Для прекращения цикла имеется перегруженный оператор,
         // принимающий Action<int, ParallelLoopState>

         Parallel.For(0, MAX_NUMBER, (i, loopState) =>
         {
            if (((uint)i).IsPrime())
            {
               Interlocked.Increment(ref primesFound);
               if (primesFound > MAX_PRIMES)
               {
                  Console.WriteLine("Last prime found: {0:N0}", i);
                  loopState.Stop();
               }
            }
         });
         watch.Stop();
         Console.WriteLine("Found {0:N0} primes in {1}",
            primesFound, watch.Elapsed);

         Console.ReadKey();
      }
   }
}
