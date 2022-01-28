/**
 * Выдача разрешений на блокировку через семафоры
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSample
{
   class Program
   {
      private const int TaskCount = 6;
      private const int SemaphoreCount = 3;

      static void Main()
      {
         var semaphore = new SemaphoreSlim(SemaphoreCount, SemaphoreCount);
         var tasks = new Task[TaskCount];

         for (int i = 0; i < TaskCount; i++)
         {
            tasks[i] = Task.Run(() => TaskMain(semaphore));
         }

         Task.WaitAll(tasks);
         Console.WriteLine("All tasks are finished");
      }

      private static void TaskMain(SemaphoreSlim semaphore)
      {
         bool isCompleted = false;
         while (!isCompleted)
         {
            if (semaphore.Wait(600))
            {
               try
               {
                  Console.WriteLine("Task {0} locks the semaphore", Task.CurrentId);
                  Thread.Sleep(2000);
               }
               finally
               {
                  Console.WriteLine("Task {0} releases the semaphore", Task.CurrentId);
                  semaphore.Release();
                  isCompleted = true;
               }
            }
            else
            {
               Console.WriteLine("Timeout for task {0}; wait again", Task.CurrentId);
            }
         }
      }
   }
}
