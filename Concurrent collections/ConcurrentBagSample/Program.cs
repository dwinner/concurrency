/**
 * Беспорядочная потокобезопасная коллекция
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentBagSample
{
   internal static class Program
   {
      private static void Main()
      {
         var sharedBag = new ConcurrentBag<int>();
         for (int i = 0; i < 1000; i++)
         {
            sharedBag.Add(i);
         }

         int itemCount = 0;

         var tasks = new Task[10];
         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               while (sharedBag.Count > 0)
               {
                  int bagElement;
                  if (sharedBag.TryTake(out bagElement))
                  {
                     Interlocked.Increment(ref itemCount);
                  }
               }
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);

         Console.WriteLine("Items processed: {0}", itemCount);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}