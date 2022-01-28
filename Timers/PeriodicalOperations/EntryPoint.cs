/**
 * Периодические вычислительные операции
 */

using System;
using System.Threading;

namespace _14_PeriodicalOperations
{
   class EntryPoint
   {
      static void Main()
      {
         TimerDemo.Go();
         Console.ReadKey();
      }
   }

   internal static class TimerDemo
   {
      private static Timer _timer;

      public static void Go()
      {
         Console.WriteLine("Main thread: starting a timer");
         using (_timer = new Timer(ComputeBoundOperation, 5, 0, Timeout.Infinite))
         {
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(10000); // Имитация другой работы (10 секунд)
         }  // Отмена таймера методом Dispose
      }

      // Сигнатура этого метода должна соответствовать сигнатуре делегата TimerCallback
      private static void ComputeBoundOperation(object state)
      {
         // Этот метод выполняется потоком из пула
         Console.WriteLine("In ComputeBoundOp: state={0}", state);
         Thread.Sleep(1000);  // Имитация другой работы (1 секунда)
         _timer.Change(2000, Timeout.Infinite); // Заставляем таймер снова вызвать метод через 2 секунды

         // Когда метод возвращает управление, поток
         // возвращается в пул и ожидает следующего задания
      }
   }
}
