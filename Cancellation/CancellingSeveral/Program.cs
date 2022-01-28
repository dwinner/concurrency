/**
 * Отмена нескольких задач по одному токену
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingSeveral
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         CancellationToken token = tokenSource.Token;

         var task1 = new Task(() =>
         {
            for (int i = 0; i < int.MaxValue; i++)
            {
               token.ThrowIfCancellationRequested();
               Console.WriteLine("Task 1 - Int value {0}", i);
            }
         }, token);

         var task2 = new Task(() =>
         {
            token.ThrowIfCancellationRequested();
            for (int i = 0; i < int.MaxValue; i++)
            {
               token.ThrowIfCancellationRequested();
               Console.WriteLine("Task 2 - Int value {0}", i);
            }
         }, token);

         Console.WriteLine("Press enter to start tasks");
         Console.WriteLine("Press enter again to cancel tasks");
         Console.ReadLine();

         task1.Start();
         task2.Start();

         Console.ReadLine();

         Console.WriteLine("Cancelling tasks");
         tokenSource.Cancel();

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}