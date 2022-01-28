/**
 * Определение, была ли отменена задача
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DetectIfCanceled
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource1 = new CancellationTokenSource();
         CancellationToken token1 = tokenSource1.Token;

         // NOTE: Задача, которая успеет завершиться
         var task1 = new Task(() =>
         {
            for (int i = 0; i < 10; i++)
            {
               token1.ThrowIfCancellationRequested();
               Console.WriteLine("Task 1 - Int value {0}", i);
            }
         }, token1);

         var tokenSource2 = new CancellationTokenSource();
         CancellationToken token2 = tokenSource2.Token;

         // NOTE: Задача, которая будет отменена
         var task2 = new Task(() =>
         {
            for (int i = 0; i < int.MaxValue; i++)
            {
               token2.ThrowIfCancellationRequested();
               Console.WriteLine("Task 2 - Int value {0}", i);
            }
         }, token2);

         task1.Start();
         task2.Start();

         tokenSource2.Cancel();

         Console.WriteLine("Task 1 cancelled? {0}", task1.IsCanceled);
         Console.WriteLine("Task 2 cancelled? {0}", task2.IsCanceled);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}