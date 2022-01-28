using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AsynchronousQueues
{
   internal static class AsyncQueueAsBufferBlock
   {
      private static readonly BufferBlock<int> AsyncQueue;

      static AsyncQueueAsBufferBlock()
      {
         AsyncQueue = new BufferBlock<int>();
      }

      internal static async void Go()
      {
         await Produce();
         await Consume();
      }

      private static async Task Produce()
      {
         await AsyncQueue.SendAsync(7);
         await AsyncQueue.SendAsync(13);

         AsyncQueue.Complete();
      }

      private static async Task Consume()
      {
         // Если consumer один
         while (await AsyncQueue.OutputAvailableAsync())
         {
            Console.WriteLine(await AsyncQueue.ReceiveAsync());
         }

         // Если consumer'ов несколько
         while (true)
         {
            int item;

            try
            {
               item = await AsyncQueue.ReceiveAsync();
            }
            catch (InvalidOperationException)
            {
               break;
            }

            Console.WriteLine(item);
         }
      }
   }
}