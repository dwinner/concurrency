/**
 * Собственная реализация легковесной спин-блокировки
 */

using System;
using System.IO;
using System.Threading;

namespace _08_CustomSpinLock
{
   static class EntryPoint
   {
      private static readonly Random Random = new Random();
      private static readonly MySpinLock LogLock = new MySpinLock(10);

      private static readonly StreamWriter FsLogWriter =
          new StreamWriter(File.Open("log.txt", FileMode.Append, FileAccess.Write, FileShare.None));

      private static void RndThreadFunc()
      {
         using (new MySpinLockManager(LogLock))
         {
            Console.WriteLine("Поток {0} запускается", Thread.CurrentThread.ManagedThreadId);
            FsLogWriter.WriteLine("Поток {0} запускается", Thread.CurrentThread.ManagedThreadId);
            FsLogWriter.Flush();
         }
         int time = Random.Next(10, 200);
         Thread.Sleep(time);
         using (new MySpinLockManager(LogLock))
         {
            Console.WriteLine("Поток {0} завершается", Thread.CurrentThread.ManagedThreadId);
            FsLogWriter.WriteLine("Поток {0} завершается", Thread.CurrentThread.ManagedThreadId);
            FsLogWriter.Flush();
         }
      }

      static void Main()
      {
         // Запустить потоки, ожидающие в течение случайного периода времени.
         var rndThreads = new Thread[50];
         for (uint i = 0; i < 50; ++i)
         {
            rndThreads[i] = new Thread(RndThreadFunc);
            rndThreads[i].Start();
         }

         Console.ReadKey();
      }
   }

   public class MySpinLock
   {
      private /*volatile*/ int _theLock;
      private readonly int _spinWait;

      public MySpinLock(int spinWait)
      {
         _spinWait = spinWait;
      }

      public void Enter()
      {
         while (Interlocked.CompareExchange(ref _theLock, 1, 0) == 1)
         {
            Thread.Sleep(_spinWait);    // Блокировка занята, ожидать
         }
      }

      public void Exit()
      {
         Interlocked.CompareExchange(ref _theLock, 0, 1);
      }
   }

   public class MySpinLockManager : IDisposable
   {
      private readonly MySpinLock _spinLock;

      public MySpinLockManager(MySpinLock spinLock)
      {
         _spinLock = spinLock;
         _spinLock.Enter();
      }

      public void Dispose()
      {
         _spinLock.Exit();
      }
   }
}
