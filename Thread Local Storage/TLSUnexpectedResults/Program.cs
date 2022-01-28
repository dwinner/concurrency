/**
 * Неожиданные результаты от использования TLS
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _04_TLSUnexpectedResults
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var tasks = new Task<int>[10];
         var threadLocal = new ThreadLocal<int>(() =>
         {
            Console.WriteLine("Value factory called for value: {0}", account.Balance);
            return account.Balance;
         });

         for (var i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task<int>(() =>
            {
               for (var j = 0; j < 1000; j++)
               {
                  threadLocal.Value++;
               }

               return threadLocal.Value;
            });

            tasks[i].Start();
         }

         foreach (var task in tasks)
         {
            account.Balance += task.Result;
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