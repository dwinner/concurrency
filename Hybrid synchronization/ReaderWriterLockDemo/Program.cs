/**
 * Использование блокировки чтения-записи
 */

using System;
using System.Text;
using System.Threading;

namespace ReaderWriterLockDemo
{
   class Program
   {
      private const int MAX_VALUES = 25;
      private static readonly int[] array = new int[MAX_VALUES];  // Shared memory
      private static readonly ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();

      static void Main()
      {
         ThreadPool.QueueUserWorkItem(WriteThread);

         for (int i = 0; i < 3; i++)
         {
            ThreadPool.QueueUserWorkItem(ReadThread);
         }

         Console.ReadKey();
      }

      static void WriteThread(object state)  // Метод пишущего потока
      {
         int id = Thread.CurrentThread.ManagedThreadId;
         for (int i = 0; i < MAX_VALUES; ++i)
         {
            try
            {
               readWriteLock.EnterWriteLock();
               try
               {
                  Console.WriteLine("Entered WriteLock on thread {0}", id);
                  array[i] = i * i;
                  Console.WriteLine("Added {0} to array on thread {1}", array[i], id);
                  Console.WriteLine("Exiting WriteLock on thread {0}", id);

                  Thread.Sleep(1000);
               }
               finally
               {
                  readWriteLock.ExitWriteLock();
               }
            }
            catch (LockRecursionException lockRecursionEx)
            {
               Console.WriteLine(lockRecursionEx);
            }
            catch (SynchronizationLockException syncLockEx)
            {
               Console.WriteLine(syncLockEx);
            }
         }
      }

      static void ReadThread(object state)   // Метод читающего потока
      {
         int id = Thread.CurrentThread.ManagedThreadId;
         for (int i = 0; i < MAX_VALUES; ++i)
         {
            try
            {
               readWriteLock.EnterReadLock();
               try
               {
                  Console.WriteLine("Entered ReadLock on thread {0}", id);
                  StringBuilder sb = new StringBuilder();
                  for (int j = 0; j < i; j++)
                  {
                     if (sb.Length > 0) sb.Append(", ");
                     sb.Append(array[j]);

                  }
                  Console.WriteLine("Array: {0} on thread {1}", sb, id);
                  Console.WriteLine("Exiting ReadLock on thread {0}", id);
                  Thread.Sleep(1000);
               }
               finally
               {
                  readWriteLock.ExitReadLock();
               }
            }
            catch (LockRecursionException lockRecursionEx)
            {
               Console.WriteLine(lockRecursionEx);
            }
            catch (SynchronizationLockException syncLockEx)
            {
               Console.WriteLine(syncLockEx);
            }
         }
      }
   }
}
