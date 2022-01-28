/**
 * Мониторинг сигнального события
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitHandleMonitoring
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
               if (token.IsCancellationRequested)
               {
                  Console.WriteLine("Task cancel detected");
                  throw new OperationCanceledException(token);
               }

               Console.WriteLine("Int value {0}", i);
            }
         }, token);

         var task2 = new Task(() =>
         {
            token.WaitHandle.WaitOne(); // Ждем сигнала на WaitHandle
            Console.WriteLine(">>>>> Wait handle released");
         });

         Console.WriteLine("Press enter to start task");
         Console.WriteLine("Press enter again to cancel task");
         Console.ReadLine();

         task1.Start();
         task2.Start();

         Console.ReadLine();

         Console.WriteLine("Cancelling task");
         tokenSource.Cancel();

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}