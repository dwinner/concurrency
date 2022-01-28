using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SingleActionBlockLoadBalance
{
   public class LoadBalancerAsync<TInput, TOutput>
   {
      private readonly ActionBlock<WorkItem> _processorBlock;
      private readonly ConcurrentQueue<Func<TInput, TOutput>> _processorQueue;

      public LoadBalancerAsync(IReadOnlyCollection<Func<TInput, TOutput>> processors, int maxQueueLength)
      {
         _processorQueue = new ConcurrentQueue<Func<TInput, TOutput>>(processors);
         _processorBlock = new ActionBlock<WorkItem>((Action<WorkItem>) ProcessWorkItem,
            new ExecutionDataflowBlockOptions
            {
               BoundedCapacity = maxQueueLength,
               MaxDegreeOfParallelism = processors.Count
            });
      }

      private void ProcessWorkItem(WorkItem workItem)
      {
         if (!_processorQueue.TryDequeue(out var operation))
            return;

         try
         {
            Task.Run(() => workItem.Tsc.SetResult(operation(workItem.Input)));
         }
         catch (Exception error)
         {
            workItem.Tsc.SetException(error);
         }

         _processorQueue.Enqueue(operation);
      }

      public Task<TOutput> DoAsync(TInput input)
      {
         var tcs = new TaskCompletionSource<TOutput>();
         var wi = new WorkItem {Input = input, Tsc = tcs};

         if (!_processorBlock.Post(wi))
            tcs.SetException(new TooBusyException());

         return tcs.Task;
      }

      private class WorkItem
      {
         public TInput Input { get; set; }
         public TaskCompletionSource<TOutput> Tsc { get; set; }
      }
   }
}