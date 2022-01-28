/**
 * Фазовый синхронизатор задач
 */

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _11_BarrierSample
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем массив банковских счетов
         var accounts = new BankAccount[5];
         for (int i = 0; i < accounts.Length; i++)
         {
            accounts[i] = new BankAccount();
         }

         int totalBalance = 0; // Создаем счетчик для суммарного кол-ва балансов

         // Создаем барьер для задач обработки счетов
         var barrier = new Barrier(accounts.Length, aBarrier =>
         {
            totalBalance = accounts.Sum(account => account.Balance);
            Console.WriteLine("Total balance: {0}", totalBalance);
         });

         var tasks = new Task[accounts.Length]; // Определяем массив задач

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(o =>
            {
               var account = o as BankAccount;
               if (account != null)
               {
                  #region Первая фаза: увеличение баланса

                  var rnd = new Random();
                  for (int j = 0; j < 1000; j++)
                  {
                     account.Balance += rnd.Next(1, 100);
                  }

                  Console.WriteLine("Task {0}, phase {1} ended", Task.CurrentId, barrier.CurrentPhaseNumber);
                  barrier.SignalAndWait(); // Посылаем сигнал на барьер

                  #endregion

                  #region Вторая фаза: изменяем баланс относительно суммарного

                  account.Balance -= (totalBalance - account.Balance) / 10;
                  Console.WriteLine("Task {0}, phase {1} ended", Task.CurrentId, barrier.CurrentPhaseNumber);
                  barrier.SignalAndWait(); // Посылаем сигнал на барьер

                  #endregion
               }
            }, accounts[i]);
         }

         foreach (Task task in tasks)
         {
            task.Start();
         }

         Task.WaitAll(tasks);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}