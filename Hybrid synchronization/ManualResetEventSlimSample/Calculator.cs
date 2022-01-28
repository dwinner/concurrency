using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimSample
{
   public class Calculator
   {
      private readonly ManualResetEventSlim _eventSlim;

      public int Result { get; private set; }

      public Calculator(ManualResetEventSlim eventSlim)
      {
         _eventSlim = eventSlim;
      }

      public void Calculation(int x, int y)
      {
         Console.WriteLine("Task {0} starts calculation", Task.CurrentId);
         Thread.Sleep(new Random().Next(3000));
         Result = x + y;
         // Сигнализировать о завершении.
         Console.WriteLine("Task {0} is ready", Task.CurrentId);
         _eventSlim.Set();
      }
   }
}