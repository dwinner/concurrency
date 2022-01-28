/**
 * Отмена параллельного цикла
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _09_CancellingParallelLoop
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         Task.Factory.StartNew(() =>
         {
            Thread.Sleep(5000);
            tokenSource.Cancel();
            Console.WriteLine("Token cancelled");
         }, tokenSource.Token);

         var loopOptions = new ParallelOptions
         {
            CancellationToken = tokenSource.Token
         };

         try
         {
            Parallel.For(0, Int64.MaxValue, loopOptions, index =>
            {
               var result = Math.Pow(index, 3);
               Console.WriteLine("Index {0}, Result {1}", index, result);
               Thread.Sleep(100);
            });
         }
         catch (OperationCanceledException)
         {
            Console.WriteLine("Caught cancellation exception...");
         }
      }
   }
}