using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsynchronousQueues
{
   internal static class AsyncProducerConsumerQueueSample
   {
      private readonly static AsyncProducerConsumerQueue<int> AsyncQueue = new AsyncProducerConsumerQueue<int>();

      internal static async void Go()
      {
         await Task.WhenAll(Produce(), Consume());
      }

      private static async Task Consume()
      {
         while (await AsyncQueue.OutputAvailableAsync())
         {
            Console.WriteLine(await AsyncQueue.DequeueAsync());
         }
      }

      private static async Task Produce()
      {
         await AsyncQueue.EnqueueAsync(7);
         await AsyncQueue.EnqueueAsync(13);

         AsyncQueue.CompleteAdding();
      }
   }
}