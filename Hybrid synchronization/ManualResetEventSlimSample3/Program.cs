/**
 * Сигнальное событие ручного сброса
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _15_ManualResetEventSlimSample
{
   internal static class Program
   {
      private static void Main()
      {
         var manualResetEvent = new ManualResetEventSlim(); // Создаем примитив

         var tokenSource = new CancellationTokenSource();

         // Создаем и запускаем задачу, которая будет ждать поступления сигнального события
         Task waitingTask = Task.Factory.StartNew(() =>
         {
            while (true)
            {
               manualResetEvent.Wait(tokenSource.Token);
               Console.WriteLine("Waiting task active");
            }
         }, tokenSource.Token);

         // Создаем и запускаем задачу, которая будет отправлять сигнал
         Task signallingTask = Task.Factory.StartNew(() =>
         {
            var rnd = new Random();
            while (!tokenSource.Token.IsCancellationRequested)
            {
               tokenSource.Token.WaitHandle.WaitOne(rnd.Next(500, 2000));
               manualResetEvent.Set(); // Посылаем сигнал
               Console.WriteLine("Event set");
               tokenSource.Token.WaitHandle.WaitOne(rnd.Next(500, 2000));
               manualResetEvent.Reset(); // Ручной сброс события
               Console.WriteLine("Event reset");
            }

            // Здесь мы уже знаем, что задача была отменена
            tokenSource.Token.ThrowIfCancellationRequested();
         }, tokenSource.Token);

         Console.WriteLine("Press enter to cancel tasks");
         Console.ReadLine();

         tokenSource.Cancel();
         try
         {
            Task.WaitAll(waitingTask, signallingTask);
         }
         catch (AggregateException)
         {
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}