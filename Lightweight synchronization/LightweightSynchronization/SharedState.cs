using System.Threading;

namespace LightweightSynchronization
{
   public class SharedState
   {
      private int _state;

      public int State
      {
         get { return _state; }
      }

      public int IncrementState()
      {
         return Interlocked.Increment(ref _state);
      }
   }
}