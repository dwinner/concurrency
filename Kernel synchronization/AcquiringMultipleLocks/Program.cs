/**
 * Запрос множественных блокировок на объектах Mutex'а
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _10_AcquiringMultipleLocks
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем два банковских счета
         var account1 = new BankAccount();
         var account2 = new BankAccount();

         // Создаем два Mutex'а
         var mutex1 = new Mutex();
         var mutex2 = new Mutex();

         // Создаем задачу, обновляющую балланс первого счета
         var task1 = new Task(() =>
         {
            for (int i = 0; i < 1000; i++)
            {
               bool lockAcquired = mutex1.WaitOne();
               try
               {
                  account1.Balance++;
               }
               finally
               {
                  if (lockAcquired)
                  {
                     mutex1.ReleaseMutex();
                  }
               }
            }
         });

         // Создаем задачу, обновляющую балланс второго счета
         var task2 = new Task(() =>
         {
            for (int i = 0; i < 1000; i++)
            {
               bool lockAcquired = mutex2.WaitOne();
               try
               {
                  account2.Balance += 2;
               }
               finally
               {
                  if (lockAcquired)
                  {
                     mutex2.ReleaseMutex();
                  }
               }
            }
         });

         // Создаем задачу, обновляющую балланс двух счетов
         var task3 = new Task(() =>
         {
            for (int i = 0; i < 1000; i++)
            {
               bool lockAcquired = WaitHandle.WaitAll(new WaitHandle[] { mutex1, mutex2 });
               try
               {
                  account1.Balance++;
                  account2.Balance--;
               }
               finally
               {
                  if (lockAcquired)
                  {
                     mutex1.ReleaseMutex();
                     mutex2.ReleaseMutex();
                  }
               }
            }
         });

         task1.Start();
         task2.Start();
         task3.Start();

         Task.WaitAll(task1, task2, task3);

         Console.WriteLine("Account1 balance {0}, Account2 balance: {1}", account1.Balance, account2.Balance);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}