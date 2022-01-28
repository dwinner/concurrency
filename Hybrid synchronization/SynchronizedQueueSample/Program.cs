/**
 * Синхронная очередь на базе шаблона условной переменной
 */

using System;
using System.Threading.Tasks;

namespace SynchronizedQueueSample
{
   static class Program
   {
      static void Main()
      {
         var queue = new SynchronizedQueue<int>();

         var enqTasks = new Task[100];
         for (int i = 0; i < enqTasks.Length; i++)
         {
            int localIndex = i;
            enqTasks[i] = new Task(() => queue.Enqueue(localIndex));
            enqTasks[i].Start();
         }

         var deqTasks = new Task<int>[100];
         for (int i = 0; i < deqTasks.Length; i++)
         {
            deqTasks[i] = new Task<int>(queue.Dequeue);
            deqTasks[i].Start();
         }

         Task.WhenAll(deqTasks);

         foreach (Task<int> deqTask in deqTasks)
         {
            Console.WriteLine(deqTask.Result);
         }
      }
   }
}
