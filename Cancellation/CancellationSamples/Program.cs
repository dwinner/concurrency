/**
 * Инфраструктура отмены
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationSamples
{
   internal static class Program
   {
      private static void Main()
      {
         CancelParallelLoop();
         CancelTask();

         Console.ReadKey();
      }

      private static void CancelParallelLoop() // NOTE: Отмена параллельных циклов
      {
         var tokenSource = new CancellationTokenSource();
         tokenSource.Token.ThrowIfCancellationRequested();
         tokenSource.Token.Register(() => Console.WriteLine("** token cancelled"));

         // Произвести отмену через 500 миллисекунд
         tokenSource.CancelAfter(500);

         try
         {
            Parallel.For(0, 100,
               new ParallelOptions
               {
                  CancellationToken = tokenSource.Token
               },
               x =>
               {
                  Console.WriteLine("loop {0} started", x);
                  var sum = 0;
                  for (var i = 0; i < 100; i++)
                  {
                     Thread.Sleep(2);
                     sum += i;
                  }
                  Console.WriteLine("loop {0} finished. Sum is {1}", x, sum);
               });
         }
         catch (OperationCanceledException canceledEx)
         {
            Console.WriteLine(canceledEx.Message);
         }
      }

      private static void CancelTask() // NOTE: Отмена задачи
      {
         var tokenSource = new CancellationTokenSource();
         tokenSource.Token.Register(() => Console.WriteLine("*** task cancelled"));

         // Произвести отмену через 500 миллисекунд
         tokenSource.CancelAfter(500);

         var task = Task.Run(() =>
         {
            // Внутри задачи
            Console.WriteLine("in task");
            for (var i = 0; i < 20; i++)
            {
               Thread.Sleep(100);
               var token = tokenSource.Token;
               if (token.IsCancellationRequested)
               {
                  // Отмена запрошена, производится отмена внутри задачи.
                  Console.WriteLine("cancelling was requested, cancelling from within the task");
                  token.ThrowIfCancellationRequested();
                  break;
               }
               // Внутри цикла
               Console.WriteLine("in loop");
            }
            // Задача завершена без отмены
            Console.WriteLine("task finished without cancellation");
         }, tokenSource.Token);

         try
         {
            task.Wait( /*tokenSource.Token*/);
         }
         catch (AggregateException aggregateEx)
         {
            Console.WriteLine("exception: {0}, {1}", aggregateEx.GetType().Name, aggregateEx.Message);
            foreach (var innerEx in aggregateEx.InnerExceptions)
            {
               Console.WriteLine("inner exception: {0} {1}", innerEx.GetType().Name, innerEx.Message);
            }
         }
      }
   }
}