using System;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEventSample
{
   public class Calculator
   {
      private readonly CountdownEvent _countdown;

      public int Result { get; private set; }

      public Calculator(CountdownEvent countdown)
      {
         _countdown = countdown;
      }

      public void Calculate(int x, int y)
      {
         Console.WriteLine("Task {0} starts calculation", Task.CurrentId);
         Thread.Sleep(new Random().Next(3000));
         Result = x + y;
         // Сигнализировать о завершении
         Console.WriteLine("Task {0} is ready", Task.CurrentId);
         _countdown.Signal();
      }
   }
}