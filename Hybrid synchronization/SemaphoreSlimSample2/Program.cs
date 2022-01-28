/**
 * Семафор
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _17_SemaphoreSlimSample
{
   internal static class Program
   {
      private static void Main()
      {
         var semaphore = new SemaphoreSlim(2);

         var tokenSource = new CancellationTokenSource();

         // Создаем и запускаем задачи, которые будут ожидать поступления сигнала
         for (int i = 0; i < 10; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (true)
               {
                  semaphore.Wait(tokenSource.Token);
                  Console.WriteLine("Task {0} released", Task.CurrentId);
               }
            }, tokenSource.Token);
         }

         // Создаем и запускаем задачу, которая отправляет сигнал
         Task.Factory.StartNew(() =>
         {
            while (!tokenSource.Token.IsCancellationRequested)
            {
               tokenSource.Token.WaitHandle.WaitOne(500);
               semaphore.Release(2);
               Console.WriteLine("Semaphore released");
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