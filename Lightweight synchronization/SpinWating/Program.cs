/**
 * Зацикливание при запросе входа в критическую секцию
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _08_SpinWating
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var spinLock = new SpinLock();
         var tasks = new Task[10];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  bool lockAcquired = false;
                  try
                  {
                     spinLock.Enter(ref lockAcquired);
                     account.Balance = account.Balance + 1;
                  }
                  finally
                  {
                     if (lockAcquired)
                     {
                        spinLock.Exit();
                     }
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