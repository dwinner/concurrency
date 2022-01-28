/**
 * Синхронизация неявным объектом монитора
 */

using System;
using System.Threading.Tasks;

namespace _05_TheLockKeyword
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();

         var tasks = new Task[10];

         var lockObj = new object();

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  lock (lockObj)
                  {
                     account.Balance = account.Balance + 1;
                  }
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
      public int Balance { get; set; }
   }
}