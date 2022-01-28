/**
 * Использование локального хранилища потока в параллельных циклах
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _10_ParallelLoopWithTLS
{
   internal static class Program
   {
      private static void Main()
      {
         var total = 0;

         Parallel.For(0,
            100,
            () => 0,
            (index, loopState, tlsValue) =>
            {
               tlsValue += index;
               return tlsValue;
            },
            value => Interlocked.Add(ref total, value)
            );

         Console.WriteLine("Total: {0}", total);
      }
   }
}