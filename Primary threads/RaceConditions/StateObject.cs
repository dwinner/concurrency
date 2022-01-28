using System.Diagnostics;

namespace RaceConditions
{
   public class StateObject
   {
      private int _state;
      private readonly object _sync = new object();

      public void ChangeState(int loop)
      {
         lock (_sync)   // NOTE: Без блокировки возникнет состязание
         {
            if (_state == 5)
            {
               _state++;
               Trace.Assert(_state == 6, string.Format("Race condition occured after {0} loops", loop));
            }
            _state = 5;
         }
      }
   }
}