/**
 * Ожидание завершения на токене отмены
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PuttingTaskToSleep
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
               bool cancelled = token.WaitHandle.WaitOne(10000);
               Console.WriteLine("Task 1 - Int value {0}. Cancelled? {1}", i, cancelled);
               if (cancelled)
               {
                  throw new OperationCanceledException(token);
               }
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