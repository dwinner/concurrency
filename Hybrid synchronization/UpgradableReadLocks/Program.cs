/**
 * Блокировки чтения для обновления данных
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _14_UpgradableReadLocks
{
   internal static class Program
   {
      private static void Main()
      {
         var rwlock = new ReaderWriterLockSlim();
         var tokenSource = new CancellationTokenSource();

         int sharedData = 0;

         var readerTasks = new Task[5];

         for (int i = 0; i < readerTasks.Length; i++)
         {
            readerTasks[i] = new Task(() =>
            {
               while (true)
               {
                  rwlock.EnterReadLock(); // Запрашиваем блокировку чтения
                  Console.WriteLine("Read lock acquired - count: {0}", rwlock.CurrentReadCount);
                  Console.WriteLine("Shared data value {0}", sharedData);
                  tokenSource.Token.WaitHandle.WaitOne(1000);
                  rwlock.ExitReadLock(); // Освобождаем блокировку чтения
                  Console.WriteLine("Read lock released - count {0}", rwlock.CurrentReadCount);
                  tokenSource.Token.ThrowIfCancellationRequested();
               }
            }, tokenSource.Token);

            readerTasks[i].Start();
         }

         var writerTasks = new Task[2];
         for (int i = 0; i < writerTasks.Length; i++)
         {
            writerTasks[i] = new Task(() =>
            {
               while (true)
               {
                  rwlock.EnterUpgradeableReadLock();

                  #region Предположим есть условие, при котором нам нужно запросить блокировку записи

                  rwlock.EnterWriteLock();
                  Console.WriteLine("Write Lock acquired - waiting readers {0}, writers {1}, upgraders {2}",
                     rwlock.WaitingReadCount, rwlock.WaitingWriteCount, rwlock.WaitingUpgradeCount);
                  sharedData++; // При этом условии мы модифицируем данные
                  tokenSource.Token.WaitHandle.WaitOne(1000);
                  rwlock.ExitWriteLock();

                  #endregion

                  rwlock.ExitUpgradeableReadLock();
                  tokenSource.Token.ThrowIfCancellationRequested();
               }
            }, tokenSource.Token);

            writerTasks[i].Start();
         }

         Console.WriteLine("Press enter to cancel tasks");
         Console.ReadLine();
         tokenSource.Cancel();

         try
         {
            Task.WaitAll(readerTasks);
         }
         catch (AggregateException aggregateEx)
         {
            aggregateEx.Handle(ex => true);  // Обработать любое из возникших
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}