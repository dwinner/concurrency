/**
 * Использование барьеров для синхронизации
 */

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UsingBarrier
{
   static class Program
   {
      static void Main()
      {
         // Создаем массив банковских счетов
         var accounts = new BankAccount[5];
         for (int i = 0; i < accounts.Length; i++)
         {
            accounts[i] = new BankAccount();
         }

         // Создаем счетчик для результирующего балланса
         int totalBalance = 0;

         // Создаем барьер
         var barrier = new Barrier(5, actionBarrier =>
         {
            totalBalance = accounts.Sum(account => account.Balance); // Суммирование балланса при прохождении фазы
            Console.WriteLine("Total balance: {0}", totalBalance);
         });

         // Определяем массив задач и добавляем бизнес-логику к ним
         var tasks = new Task[5];
         for (int i = 0; i < tasks.Length; i++)
         {
            // ReSharper disable once ImplicitlyCapturedClosure
            tasks[i] = new Task(o =>
            {
               var account = o as BankAccount;
               if (account != null)
               {
                  // Первая фаза
                  var random = new Random();
                  for (int j = 0; j < 1000; j++)
                  {
                     account.Balance += random.Next(1, 100);
                  }//--

                  Console.WriteLine("Task {0}, phase {1} ended", Task.CurrentId, barrier.CurrentPhaseNumber);

                  barrier.SignalAndWait();   // Посылаем сигнал барьеру

                  // Вторая фаза
                  account.Balance -= (totalBalance - account.Balance) / 10;//--

                  Console.WriteLine("Task {0}, phase {1} ended", Task.CurrentId, barrier.CurrentPhaseNumber);

                  barrier.SignalAndWait();   // Посылаем сигнал барьеру
               }
            }, accounts[i]);
         }

         // Старт всех задач
         foreach (Task task in tasks)
         {
            task.Start();
         }

         // Ждем здесь завершения их всех
         Task.WaitAll(tasks);

         Console.WriteLine("Press enter to finish");
      }
   }
}
