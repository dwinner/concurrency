/**
 * Параллельный кэш
 */

using System;
using System.Threading.Tasks;

namespace ParallelCache
{
   static class Program
   {
      static void Main()
      {
         var cache = new ParallelCache<int, double>(key =>
         {
            Console.WriteLine("Created value for key {0}", key);
            return Math.Pow(key, 2);
         });

         for (int i = 0; i < 10; i++)
         {
            Task.Factory.StartNew(() =>
            {
               for (int j = 0; j < 20; j++)
               {
                  Console.WriteLine("Task {0} got value {1} for key {2}",
                     Task.CurrentId, cache.GetValue(j), j);
               }
            });
         }

         Console.Write("Press enter to finish");
         Console.ReadLine();
      }
   }
}
