/**
 * Простая задача продолжения
 */

using System;
using System.Threading.Tasks;

namespace _01_TaskContinuation
{
   internal static class Program
   {
      private static void Main()
      {
         var accounTask = new Task<BankAccount>(() =>
         {
            var account = new BankAccount();
            for (int i = 0; i < 1000; i++)
            {
               account.Balance++;
            }

            return account;
         });

         accounTask.ContinueWith(task => Console.WriteLine("Final Balance: {0}", task.Result.Balance));

         accounTask.Start();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}