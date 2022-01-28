/**
 * Одна задача продолжения после любой или всех выполненных
 */

using System;
using System.Threading.Tasks;

namespace _05_ManyToOne
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var tasks = new Task<int>[10];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task<int>(stateObject =>
            {
               var isolatedBalance = (int)stateObject;

               for (int j = 0; j < 1000; j++)
               {
                  isolatedBalance++;
               }

               return isolatedBalance;
            }, account.Balance);
         }

         Task continuation = Task.Factory.ContinueWhenAll(tasks, antecedents =>
         {
            foreach (var task in antecedents)
            {
               account.Balance += task.Result;
            }
         });

         foreach (var task in tasks)
         {
            task.Start();
         }

         continuation.Wait(); // Ждем результатов от задачи продолжения

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