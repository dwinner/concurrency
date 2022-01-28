using System.Threading;

namespace _15_ContextualPartitioner
{
   public class WorkItem
   {
      public int WorkDuration { get; private set; }

      public WorkItem(int workDuration)
      {
         WorkDuration = workDuration;
      }

      public void PerformWork()
      {
         Thread.Sleep(WorkDuration);
      }
   }
}