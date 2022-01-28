/**
 * Несколько задач продолжения
 */

using System;
using System.Threading.Tasks;

namespace _03_OneToMany
{
   internal static class Program
   {
      private static void Main()
      {
         var rootTask = new Task<BankAccount>(() =>
         {
            var account = new BankAccount();
            for (int i = 0; i < 1000; i++)
            {
               account.Balance++;
            }

            return account;
         });

         Task<int> conTask1 = rootTask.ContinueWith(task =>
         {
            Console.WriteLine("Interim Balance 1: {0}", task.Result.Balance);
            return task.Result.Balance * 2;
         });

         conTask1.ContinueWith(task => Console.WriteLine("Final Balance 1: {0}", task.Result));

         // Создание двух последовательных задач продолжения в один шаг
         rootTask.ContinueWith(task =>
         {
            Console.WriteLine("Internim Balance 2: {0}", task.Result.Balance);
            return task.Result.Balance / 2;
         }).ContinueWith(task => Console.WriteLine("Final Balance 2: {0}", task.Result));

         rootTask.Start();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}