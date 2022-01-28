using System.Diagnostics;

namespace RaceConditions
{
   public class SampleTask
   {
      public void RaceCondition(object state)
      {
         Trace.Assert(state is StateObject, "state must be of type StateObject");
         var stateObject = state as StateObject;
         int i = 0;
         while (true)
         {
            stateObject.ChangeState(i++);
         }
      }
   }
}