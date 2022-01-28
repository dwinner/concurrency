namespace SpinLockingViaInterlocked
{
   public class SomeResource
   {
      private SimpleSpinLock _spinLock = new SimpleSpinLock();

      public void AccessResource()
      {
         try
         {
            _spinLock.Enter();
            // Note: Здесь доступ к ресурсу в любой момент времени имеет только один поток
         }
         finally
         {
            _spinLock.Leave();
         }
      }
   }
}