/**
 * Странное поведение кода, вызванного оптимизацией компилятора
 */

using System;
using System.Threading;

namespace StrangeBehavior
{
   internal static class Program
   {
      private static bool _stopWorker = true;

      static void Main()
      {
         Console.WriteLine("Main: letting worker run for 5 seconds");
         var thread = new Thread(Worker);
         thread.Start();
         Thread.Sleep(5000);
         _stopWorker = false;
         Console.WriteLine("Main: waiting for worker to stop");
         thread.Join();
         Console.Write("Press any key to continue...");
         Console.ReadKey();
      }

      private static void Worker(object o)
      {
         int x = 0;
         while (!_stopWorker) // Note: Из-за оптимизации переменная x никогда не увеличится
         {
            x++;
         }
         Console.WriteLine("Worker: stopped when x = {0}", x); // Note: x = 0!
      }
   }
}
