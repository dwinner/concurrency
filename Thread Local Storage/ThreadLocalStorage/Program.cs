/**
 * Изоляция данных через локальное хранилище потока
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _03_ThreadLocalStorage
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();

         var tasks = new Task<int>[10];

         var tls = new ThreadLocal<int>(); // Создаем локальное хранилище потока

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task<int>(stateObj =>
            {
               tls.Value = (int)stateObj; // Получаем объект состояния и используем его для установки в TLS
               for (int j = 0; j < 1000; j++)
               {
                  tls.Value++; // Обновляем баланс TLS
               }

               return tls.Value;
            }, account.Balance);

            tasks[i].Start();
         }

         foreach (var aTask in tasks)
         {
            account.Balance += aTask.Result;
         }

         Console.WriteLine("Expected value {0}, Balance: {1}", 10000, account.Balance);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}