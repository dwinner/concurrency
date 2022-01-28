namespace LockSynchronization
{
   public class Job
   {
      private readonly SharedState _sharedState;
      //private readonly object _syncJob = new object();

      public Job(SharedState sharedState)
      {
         _sharedState = sharedState;
      }

      public void DoTheJob()
      {
         for (int i = 0; i < 50000; i++)
         {
            //_sharedState.IncrementState(); // NOTE: Синхронизация данных гарантирует корректный результат!
            lock (_sharedState)  // NOTE: Если блокировать по разделяемому объекту, то тоже всё Ок
            {
               _sharedState.State++;
            }
            //lock (_syncJob) // NOTE: А так нельзя делать, потому что для каждого экземпляра Job есть свой объект _syncJob!
            //{
            //   _sharedState.State++;
            //}
         }
      }
   }
}