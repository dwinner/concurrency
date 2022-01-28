/**
 * Ожидание завершения нескольких задач
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitingSeveral
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         CancellationToken token = tokenSource.Token;

         var task1 = new Task(() =>
         {
            for (int i = 0; i < 5; i++)
            {
               token.ThrowIfCancellationRequested();
               Console.WriteLine("Task 1 - Int value {0}", i);
               token.WaitHandle.WaitOne(1000);
            }
            Console.WriteLine("Task 1 complete");
         }, token);

         var task2 = new Task(() => Console.WriteLine("Task 2 complete"), token);

         task1.Start();
         task2.Start();

         Console.WriteLine("Waiting for tasks to complete.");
         Task.WaitAll(task1, task2);
         Console.WriteLine("Tasks Completed.");

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}