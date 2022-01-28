/**
 * Синхронизатор для читающих/записывающих потоков
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterSample
{
   class Program
   {
      private static readonly List<int> Items = new List<int> { 0, 1, 2, 3, 4, 5 };

      private static readonly ReaderWriterLockSlim ReaderWriterLock =
         new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

      private const int TaskCount = 7;

      static void Main()
      {
         var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
         var tasks = new Task[TaskCount];
         tasks[0] = taskFactory.StartNew(WriterMethod, 1);
         tasks[1] = taskFactory.StartNew(ReaderMethod, 1);
         tasks[2] = taskFactory.StartNew(ReaderMethod, 2);
         tasks[3] = taskFactory.StartNew(WriterMethod, 2);
         tasks[4] = taskFactory.StartNew(ReaderMethod, 3);
         tasks[5] = taskFactory.StartNew(ReaderMethod, 4);
         tasks[6] = taskFactory.StartNew(FreshDataMethod, null);

         for (int i = 0; i < TaskCount; i++)
         {
            tasks[i].Wait();
         }
      }

      static void ReaderMethod(object reader)   // NOTE: Читающий метод
      {
         try
         {
            ReaderWriterLock.EnterReadLock();
            for (int i = 0; i < Items.Count; i++)
            {
               Console.WriteLine("reader {0}, loop: {1}, item: {2}", reader, i, Items[i]);
               Thread.Sleep(40);
            }
         }
         finally
         {
            ReaderWriterLock.ExitReadLock();
         }
      }

      static void WriterMethod(object writer)   // NOTE: Записывающий метод
      {
         try
         {
            while (!ReaderWriterLock.TryEnterWriteLock(TimeSpan.FromMilliseconds(50)))
            {
               Console.WriteLine("Writer {0} waiting for the write lock", writer);  // Записывающий поток ожидает блокировку записи
               Console.WriteLine("Current reader count: {0}", ReaderWriterLock.CurrentReadCount); // Вывод количества считывающих потоков
            }
            for (int i = 0; i < Items.Count; i++)
            {
               Items[i]++;
               Thread.Sleep(50);
               Console.WriteLine("Writer {0} finished", writer);
            }
         }
         finally
         {
            ReaderWriterLock.ExitWriteLock();
         }
      }

      static void FreshDataMethod(object state) // NOTE: Читающий в режиме обновления
      {
         try
         {
            ReaderWriterLock.EnterUpgradeableReadLock();
            Console.WriteLine("Upgradable lock entered");
            Items.ForEach(Console.WriteLine);
         }
         finally
         {
            ReaderWriterLock.ExitUpgradeableReadLock();
            Console.WriteLine("Upgradable lock exited");
         }
      }
   }
}
