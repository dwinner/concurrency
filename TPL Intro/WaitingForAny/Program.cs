/**
 * Ожидание завершения любой из задач
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitingForAny
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
         int taskIndex = Task.WaitAny(task1, task2);
         Console.WriteLine("Task Completed. Index: {0}", taskIndex);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}