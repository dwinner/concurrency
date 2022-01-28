/**
 * Таймеры
 */

using System;
using System.Threading;

namespace _21_Timer
{
   class Program
   {
      private static void TimerProc(object state)
      {
         Console.WriteLine("Текущее время {0} на потоке {1}",
            DateTime.Now,
            Thread.CurrentThread.ManagedThreadId);
         Thread.Sleep(20000);
      }

      static void Main()
      {
         Console.WriteLine("Нажмите <Enter> для завершения\n\n");
         var myTimer = new Timer(new TimerCallback(TimerProc), null, 0, 2000);
         Console.ReadLine();
         myTimer.Dispose();
      }
   }
}
