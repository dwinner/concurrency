/**
 * Проблема с данными
 */

using System;
using System.Threading.Tasks;

namespace _01_TroubleWithData
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var tasks = new Task[10];

         for (var i = 0; i < 10; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (var j = 0; j < 1000; j++)
               {
                  account.Balance = account.Balance + 1;
               }
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);
         Console.WriteLine("Expected value {0}, Counter value: {1}", 10000, account.Balance);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}