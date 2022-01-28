/**
 * Сигнальное событие ручного сброса
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _16_AutoResetEventSample
{
   internal static class Program
   {
      private static void Main()
      {
         var autoResetEvent = new AutoResetEvent(false);

         var tokenSource = new CancellationTokenSource();

         for (int i = 0; i < 3; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (!tokenSource.Token.IsCancellationRequested)
               {
                  autoResetEvent.WaitOne(); // Ждем сигнала
                  Console.WriteLine("Task {0} released", Task.CurrentId);
               }

               tokenSource.Token.ThrowIfCancellationRequested();
            }, tokenSource.Token);
         }

         Task.Factory.StartNew(() =>
         {
            while (!tokenSource.Token.IsCancellationRequested)
            {
               tokenSource.Token.WaitHandle.WaitOne(500);
               autoResetEvent.Set(); // Посылаем сигнал
               Console.WriteLine("Event set");
            }

            tokenSource.Token.ThrowIfCancellationRequested();
         }, tokenSource.Token);

         Console.WriteLine("Press enter to cancel tasks");
         Console.ReadLine();

         tokenSource.Cancel();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}