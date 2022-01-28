/**
 * Lazy producer consumer
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LazyProducerConsumer
{
   internal static class LazyProducerEagerConsumerEx
   {
      private static void Main()
      {
         var consumerBlock = new ActionBlock<int>(index => Consume(index));
         PrintThreadPoolUsage(nameof(Main));

         for (var i = 0; i < 5; i++)
         {
            consumerBlock.Post(i);
            Thread.Sleep(1000);
            PrintThreadPoolUsage("loop");
         }

         // Tell the block no more items will be coming
         consumerBlock.Complete();

         // Wait for the block to shutdown
         consumerBlock.Completion.Wait();
      }

      private static void Consume(int val)
      {
         PrintThreadPoolUsage(nameof(Consume));
         Console.WriteLine("{0}:{1} is thread pool thread {2}",
            Task.CurrentId,
            val,
            Thread.CurrentThread.IsThreadPoolThread);
      }

      private static void PrintThreadPoolUsage(string label)
      {
         ThreadPool.GetAvailableThreads(out var cpu, out var io);
         Console.WriteLine("{0}:CPU:{1},IO:{2}", label, cpu, io);
      }
   }
}