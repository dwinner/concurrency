using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace NonGreedyLinkingBlockSample
{
   internal static class Program
   {
      private static void Main()
      {
         //var greedy = new ExecutionDataflowBlockOptions();
         var nonGreedy = new ExecutionDataflowBlockOptions {BoundedCapacity = 1};

         var options = nonGreedy;

         var firstBlock = new ActionBlock<int>(i => Do(i, 1, 2), options);
         var secondBlock = new ActionBlock<int>(i => Do(i, 2, 1), options);
         var thirdBlock = new ActionBlock<int>(i => Do(i, 3, 2), options);

         var transform = new TransformBlock<int, int>(i => i * 2);

         transform.LinkTo(firstBlock);
         transform.LinkTo(secondBlock);
         transform.LinkTo(thirdBlock);

         for (var i = 0; i <= 10; i++)
            transform.Post(i);

         Console.ReadLine();
      }

      private static void Do(int workItem, int nWorker, int busyTimeInSeconds)
      {
         Console.WriteLine("Worker {0} Busy processing {1}", nWorker, workItem);
         Thread.Sleep(busyTimeInSeconds * 1000);
         Console.WriteLine("Worker {0} Done", nWorker);
      }
   }
}