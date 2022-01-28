using System;

namespace Deadlock
{
   public class SampleTask
   {
      private readonly StateObject _firstStateObject;
      private readonly StateObject _secondStateObject;

      public SampleTask(StateObject firstStateObject, StateObject secondStateObject)
      {
         _firstStateObject = firstStateObject;
         _secondStateObject = secondStateObject;
      }

      public void Deadlock1()
      {
         int i = 0;
         while (true)
         {
            lock (_firstStateObject)
            {
               lock (_secondStateObject)
               {
                  _firstStateObject.ChangeState(i);
                  _secondStateObject.ChangeState(i++);
                  Console.WriteLine("still running, {0}", i);
               }
            }
         }
      }

      public void Deadlock2()
      {
         int i = 0;
         while (true)
         {
            lock (_secondStateObject)
            {
               lock (_firstStateObject)
               {
                  _firstStateObject.ChangeState(i);
                  _secondStateObject.ChangeState(i++);
                  Console.WriteLine("still running, {0}", i);
               }
            }
         }
      }
   }
}