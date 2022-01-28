/**
 * Возвращение результатов из задачи
 */

using System;
using System.Threading.Tasks;

namespace _02_ReturningResult
{
   internal static class Program
   {
      private static void Main()
      {
         var task = new Task<BankAccount>(() =>
         {
            var account = new BankAccount();
            for (int i = 0; i < 1000; i++)
            {
               account.Balance++;
            }

            return account;
         });

         Task<int> continuationTask = task.ContinueWith(aTask =>
         {
            Console.WriteLine("Interim Balance: {0}", aTask.Result.Balance);
            return aTask.Result.Balance * 2;
         });

         task.Start();

         Console.WriteLine("Final balance: {0}", continuationTask.Result);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}