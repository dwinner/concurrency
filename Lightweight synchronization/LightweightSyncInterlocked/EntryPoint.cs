/**
 * Легковесная синхронизация с помощью Interlocked
 */

using System;
using System.Threading;

namespace _07_LightweightSyncInterlocked
{
   class EntryPoint
   {
      private static volatile int _numberThreads;
      private static readonly Random Random = new Random();

      private static void RndThreadFunc()
      {
         // Управлять счетчиком потоков и ожидать произвольный
         // промежуток времени от 1 до 12 секунд.
         Interlocked.Increment(ref _numberThreads);
         try
         {
            int time = Random.Next(1000, 12000);    // Note: теоретически возможно несогласованное состояние
            Thread.Sleep(time);
         }
         finally
         {
            Interlocked.Decrement(ref _numberThreads);
         }
      }

      private static void RptThreadFunc()
      {
         while (true)
         {
            int threadCount = Interlocked.CompareExchange(ref _numberThreads, 0, 0);
            Console.WriteLine("{0} потоков активно", threadCount);
            Thread.Sleep(1000);
         }
      }

      static void Main()
      {
         // Запустить потоки отчетов.
         var reporter = new Thread(RptThreadFunc) { IsBackground = true };
         reporter.Start();

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
