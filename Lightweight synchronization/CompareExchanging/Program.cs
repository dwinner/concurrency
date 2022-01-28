/**
 * Синхронизация данных сравнением
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _07_CompareExchanging
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var tasks = new Task[10];

         for (var i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               var startBalance = account.Balance; // Получаем локальную копию совместно используемых данных
               var localBalance = startBalance; // Создаем локальную рабочую копию совместно используемых данных

               for (var j = 0; j < 1000; j++)
               {
                  localBalance++; // Обновляем локальную копию баланса
               }

               // Проверяем, обновились ли совместно используемые данные с момента запуска задачи.
               // Если нет, то обновляем эти данные нашими локальными
               var sharedData = Interlocked.CompareExchange(ref account.Balance, localBalance, startBalance);
               Console.WriteLine(sharedData == startBalance ? "Shared data updated OK" : "Shared data changed");
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);
         Console.WriteLine("Expected value {0}, Balance: {1}", 10000, account.Balance);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance;
   }
}