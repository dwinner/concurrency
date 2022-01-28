namespace LightweightSynchronization
{
   public class Job
   {
      private readonly SharedState _sharedState;

      public Job(SharedState sharedState)
      {
         _sharedState = sharedState;
      }

      public void DoTheJob()
      {
         for (int i = 0; i < 50000; i++)
         {
            _sharedState.IncrementState();
         }
      }
   }
}