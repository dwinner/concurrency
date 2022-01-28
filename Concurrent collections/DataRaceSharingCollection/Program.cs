/**
 * Ситуация гонки на коллекциях
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataRaceSharingCollection
{
   internal static class Program
   {
      private static void Main()
      {
         var sharedQueue = new Queue<int>();
         for (int i = 0; i < 1000; i++)
         {
            sharedQueue.Enqueue(i);
         }

         int itemCount = 0;

         var tasks = new Task[10];
         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               while (sharedQueue.Count > 0)
               {
                  /*int qItem = */
                  sharedQueue.Dequeue();
                  Interlocked.Increment(ref itemCount);
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