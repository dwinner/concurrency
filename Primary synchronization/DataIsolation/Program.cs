/**
 * Изоляция данных
 */

using System;
using System.Threading.Tasks;

namespace _02_DataIsolation
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();

         var tasks = new Task<int>[10];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task<int>(o =>
            {
               var isolatedBalance = (int)o;
               for (int j = 0; j < 1000; j++)
               {
                  isolatedBalance++;
               }

               return isolatedBalance;
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