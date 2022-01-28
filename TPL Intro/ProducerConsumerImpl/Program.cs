/**
 * Простая реализация поставщика/потребителя
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _18_ProducerConsumerImpl
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем блокирующую коллекцию
         var blockingCollection = new BlockingCollection<Deposit>();

         // Создаем и запускаем поставщиков, которые будут генерировать депозиты и помещать их в коллекцию
         var producerTasks = new Task[3];
         for (int i = 0; i < producerTasks.Length; i++)
         {
            producerTasks[i] = Task.Factory.StartNew(() =>
            {
               for (int j = 0; j < 20; j++)
               {
                  var deposit = new Deposit
                  {
                     Amount = 100
                  };
                  blockingCollection.Add(deposit);
               }
            });
         }

         // Создаем продолжение Многие к Одному, которое будет посылать сигнал
         // об окончании работы задач-производителей
         Task.Factory.ContinueWhenAll(producerTasks, tasks =>
         {
            Console.WriteLine("Signalling production end");
            blockingCollection.CompleteAdding();
         });

         // Создаем банковский счет
         var account = new BankAccount();

         // Создаем поставщика, который будет обновлять баланс, основанный на депозитах
         Task consumerTask = Task.Factory.StartNew(() =>
         {
            while (!blockingCollection.IsCompleted)
            {
               Deposit deposit;
               if (blockingCollection.TryTake(out deposit))
               {
                  account.Balance += deposit.Amount;
               }
            }

            Console.WriteLine("Final Balance: {0}", account.Balance);
         });

         consumerTask.Wait();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }

   internal class Deposit
   {
      public int Amount { get; set; }
   }
}