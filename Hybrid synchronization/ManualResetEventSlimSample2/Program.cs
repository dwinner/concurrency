/**
 * Гибридная версия мануальных событий,
 * приводит к зацикливанию в пользовательском режиме.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimSample
{
   static class Program
   {
      static void Main()
      {
         // Создаем примитив
         var manualResetEvent = new ManualResetEventSlim();

         // Создаем источник управления отменой
         var tokenSource = new CancellationTokenSource();

         // Создаем и запускаем задачу, которая ждет установки события
         Task waitingTask = Task.Factory.StartNew(() =>
         {
            while (true)
            {
               // Ждем на примитиве
               manualResetEvent.Wait(tokenSource.Token);

               // Печатаем сообщение по установке события
               Console.WriteLine("Waiting task active");
            }
            // ReSharper disable once FunctionNeverReturns
         }, tokenSource.Token);

         // Создаем и запускаем сигнальную задачу
         Task signallingTask = Task.Factory.StartNew(() =>
         {
            // Создаем случайное число для периода сна
            var random = new Random();

            // Зацикливаемся, пока задача не получит запрос на отмену
            while (!tokenSource.Token.IsCancellationRequested)
            {
               // блокируемся на неопределенный период
               tokenSource.Token.WaitHandle.WaitOne(random.Next(500, 2000));

               // Устанавливаем событие
               manualResetEvent.Set();

               Console.WriteLine("Event set");
               tokenSource.Token.WaitHandle.WaitOne(random.Next(500, 2000));
               manualResetEvent.Reset();
               Console.WriteLine("Event reset");
            }

            // Если мы достигли этой точки, то знаем, что задача была завершена
            tokenSource.Token.ThrowIfCancellationRequested();
         }, tokenSource.Token);

         Console.WriteLine("Press enter to cancel the tasks");
         Console.ReadLine();

         // Отменяем источник отмены и ждем завершения задач
         tokenSource.Cancel();
         try
         {
            Task.WaitAll(waitingTask, signallingTask);
         }
         catch (AggregateException)
         {
            // Игнорируем исключения
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
