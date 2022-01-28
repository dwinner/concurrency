/**
 * Ожидание завершения задачи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitingSingle
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         CancellationToken token = tokenSource.Token;

         Task task = CreateTask(token);
         task.Start();

         Console.WriteLine("Waiting for task to complete.");
         task.Wait(token);
         Console.WriteLine("Task Completed.");

         task = CreateTask(token);
         task.Start();

         Console.WriteLine("Waiting 2 secs for task to complete.");
         bool completed = task.Wait(2000);
         Console.WriteLine("Wait ended - task completed: {0}", completed);

         task = CreateTask(token);
         task.Start();

         Console.WriteLine("Waiting 2 secs for task to complete.");
         completed = task.Wait(2000, token);
         Console.WriteLine("Wait ended - task completed: {0} task cancelled {1}",
            completed, task.IsCanceled);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }

      private static Task CreateTask(CancellationToken token)
      {
         return new Task(() =>
         {
            for (int i = 0; i < 5; i++)
            {
               token.ThrowIfCancellationRequested();
               Console.WriteLine("Task - Int value {0}", i);
               token.WaitHandle.WaitOne(1000);
            }
         }, token);
      }
   }
}