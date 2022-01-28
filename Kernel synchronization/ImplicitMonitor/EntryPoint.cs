/**
 * Неявный монитор объекта
 */

using System;
using System.Threading;

namespace _11_ImplicitMonitor
{
   class EntryPoint
   {
      private static readonly object TheLockObject = new object();
      private static int _numberThreads;
      private static readonly Random Random = new Random();

      private static void RndThreadFunc()
      {
         // Управлять счетчиком потоков и ожидать произвольный промежуток времени от 1 до 12 секунд
         lock (TheLockObject)
         {
            ++_numberThreads;
         }

         int time = Random.Next(1000, 12000);
         Thread.Sleep(time);

         lock (TheLockObject)
         {
            --_numberThreads;
         }
      }

      private static void RptThreadFunc()
      {
         while (true)
         {
            int threadCount;
            lock (TheLockObject)
            {
               threadCount = _numberThreads;
            }
            Console.WriteLine("{0} потоков активно", threadCount);
            Thread.Sleep(1000);
         }
      }

      static void Main()
      {
         // Запустить потоки отчетов
         var reporterThread = new Thread(RptThreadFunc) { IsBackground = true };
         reporterThread.Start();

         // Запустить потоки, ожидающие в течение случайного периода времени
         var rndThreads = new Thread[50];
         for (int i = 0; i < rndThreads.Length; i++)
         {
            rndThreads[i] = new Thread(RndThreadFunc);
            rndThreads[i].Start();
         }
      }
   }
}
