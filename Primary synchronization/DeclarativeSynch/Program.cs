/**
 * Декларативная синхронизация
 */

using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace _12_DeclarativeSynch
{
   internal static class Program
   {
      private static void Main()
      {
         IBankAccount syncBankAccount = new SyncBankAccountAdapter(new BankAccount());
         var tasks = new Task[10];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  syncBankAccount.IncrementBalance();
               }
            });

            tasks[i].Start();
         }

         Task.WaitAll(tasks);

         Console.WriteLine("Expected value {0}, Balance: {1}", 10000, syncBankAccount.Balance);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal interface IBankAccount
   {
      int Balance { get; set; }

      void IncrementBalance();
   }

   internal class BankAccount : IBankAccount
   {
      public int Balance { get; set; }

      public void IncrementBalance()
      {
         Balance++;
      }
   }

   [Synchronization]
   internal class SyncBankAccount : ContextBoundObject   // Proxy-класс для IBankAccount
   {
      private readonly IBankAccount _bankAccount;

      public SyncBankAccount(IBankAccount bankAccount)
      {
         _bankAccount = bankAccount;
      }

      public int Balance
      {
         get { return _bankAccount.Balance; }
         set { _bankAccount.Balance = value; }
      }

      public void IncrementBalance()
      {
         _bankAccount.IncrementBalance();
      }
   }

   internal class SyncBankAccountAdapter : IBankAccount  // Класс-адаптер для IBankAccount
   {
      private readonly SyncBankAccount _syncBankAccount;

      public SyncBankAccountAdapter(IBankAccount bankAccount)
      {
         _syncBankAccount = new SyncBankAccount(bankAccount);
      }

      public int Balance
      {
         get { return _syncBankAccount.Balance; }
         set { _syncBankAccount.Balance = value; }
      }

      public void IncrementBalance()
      {
         _syncBankAccount.IncrementBalance();
      }
   }
}