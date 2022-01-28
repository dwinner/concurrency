namespace LockSynchronization
{
   public class SharedState
   {
      private readonly object _syncObject = new object();

      public int State { get; set; }

      public void IncrementState()
      {
         lock (_syncObject)
         {
            State++;
         }
      }
   }
}