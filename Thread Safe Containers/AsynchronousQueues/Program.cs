/**
 * Асинхронная очередь
 */

using System;

namespace AsynchronousQueues
{
   internal static class Program
   {
      private static void Main()
      {
         Console.WriteLine("Buffer block idiom");
         AsyncQueueAsBufferBlock.Go();

         Console.WriteLine("Async producer/consumer idiom");
         AsyncProducerConsumerQueueSample.Go();

         Console.WriteLine("Async producer/consumer idiom (balanced way)");
         ThrottlingSample.Go();

         Console.ReadLine();
      }
   }
}