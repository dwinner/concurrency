/**
 * Ранние приемы синхронизации коллекций
 */

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace FirstGenerationSync
{
   internal static class Program
   {
      private static void Main()
      {
         Synchronized();
         SyncRootIdiom();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }

      private static void Synchronized()
      {
         Queue synchQueue = Queue.Synchronized(new Queue());
         var tasks = new Task[10];
         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 100; j++)
               {
                  synchQueue.Enqueue(j);
               }
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);
         Console.WriteLine("Items enqueued: {0}", synchQueue.Count);
      }

      private static void SyncRootIdiom()
      {
         var sharedQueue = new Queue();
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
                  lock (sharedQueue.SyncRoot)
                  {
                     if (sharedQueue.Count > 0)
                     {
                        /*var qElement = (int) */
                        sharedQueue.Dequeue();
                        Interlocked.Increment(ref itemCount);
                     }
                  }
               }
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);
         Console.WriteLine("Items processed: {0}", itemCount);
      }
   }
}