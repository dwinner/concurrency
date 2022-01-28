/**
 * Блокировки чтения/записи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _13_ReaderWriterLocks
{
   internal static class Program
   {
      private static void Main()
      {
         var rwlock = new ReaderWriterLockSlim();
         var tokenSource = new CancellationTokenSource();
         var tasks = new Task[5];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               while (true)
               {
                  rwlock.EnterReadLock(); // Запрашиваем блокировку чтения
                  Console.WriteLine("Read lock acquired - count: {0}", rwlock.CurrentReadCount);
                  tokenSource.Token.WaitHandle.WaitOne(1000); // Симулируем операцию чтения
                  rwlock.ExitReadLock(); // Освобождаем блокировку чтения
                  Console.WriteLine("Read lock released - count: {0}", rwlock.CurrentReadCount);
                  tokenSource.Token.ThrowIfCancellationRequested(); // Проверяем, была ли отмена
               }
            }, tokenSource.Token);

            tasks[i].Start();
         }

         Console.WriteLine("Press enter to acquire write lock");
         Console.ReadLine();

         Console.WriteLine("Requesting write lock");
         rwlock.EnterWriteLock();

         Console.WriteLine("Write lock acquired");
         Console.WriteLine("Press enter to release write lock");
         Console.ReadLine();
         rwlock.ExitWriteLock();

         tokenSource.Token.WaitHandle.WaitOne(2000);
         tokenSource.Cancel();

         try
         {
            Task.WaitAll(tasks);
         }
         catch (AggregateException)
         {
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}