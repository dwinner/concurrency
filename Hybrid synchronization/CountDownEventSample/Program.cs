/**
 * Сигнальное облегченное событие с обратным отсчетом
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CountDownEventSample
{
   static class Program
   {
      static void Main()
      {
         // Создаем событие обратного отсчета со счетчиком 5
         var countdown = new CountdownEvent(5);

         // Создаем случайное число, которое будет использоваться для генерации интервалов сна
         var random = new Random();

         // Создаем 5 заданий, каждое из которых будет ждать
         // случайный отрезок времени и посылать сигнал на событие
         var tasks = new Task[6];
         for (int i = 0; i < tasks.Length; i++)
         {
            // Создаем новую задачу
            tasks[i] = new Task(() =>
            {
               Thread.Sleep(random.Next(500, 1000));
               Console.WriteLine("Task {0} signalling event", Task.CurrentId);
               countdown.Signal();
            });
         }

         // Создаем финальную задачу, которая будет ждать поступления сигнала от 5 других
         // ReSharper disable once ImplicitlyCapturedClosure
         tasks[5] = new Task(() =>
         {
            Console.WriteLine("Rendezvous task waiting");
            countdown.Wait();
            Console.WriteLine("Event has been set");
         });

         // Запускаем задачи
         foreach (var task in tasks)
         {
            task.Start();
         }

         Task.WaitAll(tasks);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
