/**
 * Проверка состояния задачи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CheckIsFaulted
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         var task1 = new Task(() => { throw new NullReferenceException(); });

         var task2 = new Task(() =>
         {
            tokenSource.Token.WaitHandle.WaitOne(-1);
            throw new OperationCanceledException();
         }, tokenSource.Token);

         task1.Start();
         task2.Start();

         tokenSource.Cancel();

         try
         {
            Task.WaitAll(task1, task2);
         }
         catch (AggregateException)
         {
         }

         Console.WriteLine("Task 1 completed: {0}", task1.IsCompleted);
         Console.WriteLine("Task 1 faulted: {0}", task1.IsFaulted);
         Console.WriteLine("Task 1 cancelled: {0}", task1.IsCanceled);
         if (task1.Exception != null) Console.WriteLine(task1.Exception.GetBaseException().Message);

         Console.WriteLine("Task 2 completed: {0}", task2.IsCompleted);
         Console.WriteLine("Task 2 faulted: {0}", task2.IsFaulted);
         Console.WriteLine("Task 2 cancelled: {0}", task2.IsCanceled);
         if (task2.Exception != null) Console.WriteLine(task2.Exception.GetBaseException().Message);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}