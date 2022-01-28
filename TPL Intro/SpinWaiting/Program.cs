/**
 * Ожидание с зацикливанием
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpinWaiting
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         CancellationToken token = tokenSource.Token;

         var task1 = new Task(() =>
         {
            for (int i = 0; i < Int32.MaxValue; i++)
            {
               Thread.SpinWait(10000);
               Console.WriteLine("Task 1 - Int value {0}", i);
               token.ThrowIfCancellationRequested();
            }
         }, token);

         task1.Start();

         Console.WriteLine("Press enter to cancel token.");
         Console.ReadLine();

         tokenSource.Cancel();

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}