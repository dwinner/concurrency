using System.Threading;

namespace _16_OrderableContextPartitioner
{
   public class WorkItem
   {
      public int WorkDuraction { get; set; }

      public void PerformWork()
      {
         Thread.Sleep(WorkDuraction);
      }
   }
}