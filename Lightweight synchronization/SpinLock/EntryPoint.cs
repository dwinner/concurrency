/**
 * Легковесная Spin-блокировка
 */

using System;
using System.IO;
using System.Threading;

namespace _09_SpinLock
{
   class EntryPoint
   {
      private static readonly Random Random = new Random();
      private static SpinLock _logLock = new SpinLock(false);

      private static readonly StreamWriter FsLogWriter =
          new StreamWriter(File.Open("log.txt", FileMode.Append, FileAccess.Write, FileShare.None));

      private static void RndThreadFunc()
      {
         bool lockTaken = false;
         _logLock.Enter(ref lockTaken);
         if (lockTaken)
         {
            try
            {
               FsLogWriter.WriteLine("Поток {0} запускается", Thread.CurrentThread.ManagedThreadId);
               FsLogWriter.Flush();
            }
            finally
            {
               _logLock.Exit();
            }
         }
         int time = Random.Next(10, 200);
         Thread.Sleep(time);
         lockTaken = false;
         _logLock.Enter(ref lockTaken);
         if (!lockTaken)
         {
            return;
         }
         try
         {
            FsLogWriter.WriteLine("Поток {0} завершается", Thread.CurrentThread.ManagedThreadId);
            FsLogWriter.Flush();
         }
         finally
         {
            _logLock.Exit();
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
}
