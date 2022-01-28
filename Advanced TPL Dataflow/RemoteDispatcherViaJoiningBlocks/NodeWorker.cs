using System;
using System.Threading.Tasks;

namespace RemoteDispatcherViaJoiningBlocks
{
   public class NodeWorker : IGridNode<WorkItem>
   {
      public Task InvokeAsync(WorkItem workItem)
      {
         var result = workItem.Lhs + workItem.Rhs;
         Console.WriteLine(result);
         return Task.Delay(TimeSpan.FromSeconds(3));
      }
   }
}