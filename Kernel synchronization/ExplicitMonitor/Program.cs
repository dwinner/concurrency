/**
 * Явный монитор объекта
 */

using System;
using System.Threading;

namespace _10_ExplicitMonitor
{
   class Program
   {
      private static readonly object SyncObject = new object();
      private static int _numberThreads;
      private static readonly Random Random = new Random();

      private static void RndThreadFunc()
      {
         // Управлять счетчиком потоков и ожидать произвольный
         // промежуток времени от 1 до 12 секунд
         Monitor.Enter(SyncObject);
         try
         {
            ++_numberThreads;
         }
         finally
         {
            Monitor.Exit(SyncObject);
         }

         int time = Random.Next(1000, 12000);
         Thread.Sleep(time);

         Monitor.Enter(SyncObject);
         try
         {
            --_numberThreads;
         }
         finally
         {
            Monitor.Exit(SyncObject);
         }
      }

      private static void RptThreadFunc()
      {
         while (true)
         {
            int threadCount;
            Monitor.Enter(SyncObject);
            try
            {
               threadCount = _numberThreads;
            }
            finally
            {
               Monitor.Exit(SyncObject);
            }

            Console.WriteLine("{0} поток(ов) активно", threadCount);
            Thread.Sleep(1000);
         }
      }

      static void Main()
      {
         // Запустить потоки отчетов.
         var reporterThread = new Thread(RptThreadFunc) { IsBackground = true };
         reporterThread.Start();

         // Запустить потоки, ожидающие в течение случайного периода времени.
         var rndThreads = new Thread[50];
         for (int i = 0; i < 50; i++)
         {
            rndThreads[i] = new Thread(RndThreadFunc);
            rndThreads[i].Start();
         }

         Console.ReadKey();
      }
   }
}
