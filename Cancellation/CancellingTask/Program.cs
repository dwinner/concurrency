/**
 * Простая отмена задачи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTask
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource(); // Создаем источник для токена отмены
         CancellationToken token = tokenSource.Token; // Получаем токен отмены

         // Создаем задачу с возможностью отмены
         var task = new Task(() =>
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

         token.Register(() => Console.WriteLine(">>>> Cancellation delegate invoked"));

         Console.WriteLine("Press enter to start task");
         Console.WriteLine("Press enter again to cancel task");
         Console.ReadLine();

         task.Start();
         Console.ReadLine();
         Console.WriteLine("Cancelling task");
         tokenSource.Cancel();

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}