/**
 * Блокировки по разным объектам
 */

using System;
using System.Threading.Tasks;

namespace MultipleLocks
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();

         var lock1 = new object();
         var lock2 = new object();

         var tasks = new Task[10];

         for (int i = 0; i < 5; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  lock (lock1)
                  {
                     account.Balance++;
                  }
               }
            });
         }

         for (int i = 5; i < 10; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  lock (lock2)
                  {
                     account.Balance++;
                  }
               }
            });
         }

         foreach (Task task in tasks)
         {
            task.Start();
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