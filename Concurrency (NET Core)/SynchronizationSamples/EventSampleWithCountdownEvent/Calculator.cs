using System;
using System.Threading;
using System.Threading.Tasks;

public class Calculator
{
   private readonly CountdownEvent _cEvent;

   public Calculator(CountdownEvent ev) => _cEvent = ev;

   public int Result { get; private set; }

   public void Calculation(int x, int y)
   {
      Console.WriteLine($"Task {Task.CurrentId} starts calculation");
      Task.Delay(new Random().Next(3000)).Wait();
      Result = x + y;

      // signal the event-completed!
      Console.WriteLine($"Task {Task.CurrentId} is ready");
      _cEvent.Signal();
   }
}