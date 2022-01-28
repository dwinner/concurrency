/**
 * Передача опций параллелизма
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _05_PassingParallelOptions
{
   internal static class Program
   {
      private static void Main()
      {
         var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };

         Parallel.For(0, 10, options, index =>
         {
            Console.WriteLine("For Index {0} started", index);
            Thread.Sleep(500);
            Console.WriteLine("For Index {0} finished", index);
         });

         int[] dataElements = { 0, 2, 4, 6, 8 };

         Parallel.ForEach(dataElements, options, index =>
         {
            Console.WriteLine("ForEach Index {0} started", index);
            Thread.Sleep(500);
            Console.WriteLine("ForEach Index {0} finished", index);
         });

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}