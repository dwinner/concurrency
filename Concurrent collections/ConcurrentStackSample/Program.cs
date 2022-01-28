/**
 * Потокобезопасный стек
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentStackSample
{
   internal static class Program
   {
      private static void Main()
      {
         var sharedStack = new ConcurrentStack<int>();
         for (int i = 0; i < 1000; i++)
         {
            sharedStack.Push(i);
         }

         int itemCount = 0;

         var tasks = new Task[10];
         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               while (sharedStack.Count > 0)
               {
                  int stackElement;
                  if (sharedStack.TryPop(out stackElement))
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