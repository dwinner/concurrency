/**
 * "Легковесная" синхронизация
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _06_TheInterlockedSync
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var tasks = new Task[10];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  Interlocked.Increment(ref account.Balance);
               }
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
      public int Balance = 0;
   }
}