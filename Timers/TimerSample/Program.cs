/**
 * Варианты периодических вызовов
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoResetTimer = System.Timers.Timer;
using ThreadTimer = System.Threading.Timer;

namespace TimerSample
{
   class Program
   {
      static void Main()
      {
         ThreadingTimer();
         TimersTimer();

         Console.ReadKey();
      }

      private async static void ThreadingTimer()
      {
         await Task.Run(() =>
         {
            using (/*var thrTimer = */new ThreadTimer(
                        state => Console.WriteLine("System.Threading.Timer {0:T}", DateTime.Now),
                        null,
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)))
            {
               Thread.Sleep(15000);
            }
         });
      }

      private async static void TimersTimer()
      {
         await Task.Run(() =>
         {
            var tmTimer = new AutoResetTimer(1000) { AutoReset = true };
            tmTimer.Elapsed += (sender, args) => Console.WriteLine("System.Timers.Timer {0:T}", args.SignalTime);
            tmTimer.Start();
            Thread.Sleep(10000);
            tmTimer.Stop();
            tmTimer.Dispose();
         });
      }
   }
}
