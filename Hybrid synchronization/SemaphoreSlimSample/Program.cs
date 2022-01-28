/**
 * Облегченная оболочка для семафора
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimSample
{
   static class Program
   {
      static void Main()
      {
         // Создаем примитив
         var semaphore = new SemaphoreSlim(2);

         // Создаем токен для отмены
         var tokenSource = new CancellationTokenSource();

         // Создаем и запускаем задачу, которая ожидает получения сигнала
         for (int i = 0; i < 10; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (true)
               {
                  semaphore.Wait(tokenSource.Token);
                  Console.WriteLine("Task {0} released", Task.CurrentId);
                  // Печатаем сообщение, когда мы получили сигнал
               }
               // ReSharper disable once FunctionNeverReturns
            }, tokenSource.Token);
         }

         // Создаем и запускаем задачу, которая установит сигнал
         Task.Factory.StartNew(() =>
         {
            // зацикливаемся, пока задачу не отменят
            while (!tokenSource.Token.IsCancellationRequested)
            {
               // Засыпаем на определенный период
               tokenSource.Token.WaitHandle.WaitOne(500);

               // Сигнал на семафор
               semaphore.Release(2);
               Console.WriteLine("Semaphore released");
            }
            // Если мы дошли до этой точки, то знаем, что задача была отменена
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
