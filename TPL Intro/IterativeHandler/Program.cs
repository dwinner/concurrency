/**
 * Итеративная обработка исключений
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IterativeHandler
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         CancellationToken token = tokenSource.Token;

         var task1 = new Task(() =>
         {
            token.WaitHandle.WaitOne(-1); // Ждем всегда или пока токен не отменен
            throw new OperationCanceledException(token);
         }, token);

         var task2 = new Task(() => { throw new NullReferenceException(); });

         task1.Start();
         task2.Start();

         tokenSource.Cancel();

         try
         {
            Task.WaitAll(task1, task2);
         }
         catch (AggregateException ex)
         {
            ex.Handle(inner => inner is OperationCanceledException);
         }

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}