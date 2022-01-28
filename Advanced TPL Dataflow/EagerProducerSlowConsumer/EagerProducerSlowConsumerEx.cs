using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EagerProducerSlowConsumer
{
   internal static class EagerProducerSlowConsumerEx
   {
      private static void Main()
      {
         var blockConfig = new ExecutionDataflowBlockOptions
         {
            NameFormat = "Type:{0},Id:{1}",
            MaxDegreeOfParallelism = 2,
            //BoundedCapacity = 2
         };

         var consumerBlock = new ActionBlock<int>(i => SlowConsumer(i), blockConfig);

         for (var i = 0; i < 5; i++)
            consumerBlock.Post(i);

         consumerBlock.Complete();
         consumerBlock.Completion.Wait();
      }

      private static void SlowConsumer(int val)
      {
         Console.WriteLine("{0}: Consuming {1}", Task.CurrentId, val);
         Thread.Sleep(1000);
      }
   }
}