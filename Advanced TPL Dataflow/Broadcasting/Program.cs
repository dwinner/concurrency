using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Broadcasting
{
   internal static class Program
   {
      private static void Main()
      {
         void ConsumerOne(int obj)
         {
            Console.WriteLine("Consumer one {0}", obj);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
         }

         void ConsumerTwo(int obj)
         {
            Console.WriteLine("Consumer two {0}", obj);
            Thread.Sleep(TimeSpan.FromMilliseconds(60));
         }

         var nonGreedy = new ExecutionDataflowBlockOptions {BoundedCapacity = 1};
         var greedy = new ExecutionDataflowBlockOptions();

         var source = new BroadcastBlock<int>(i => i);

         var consumeOne = new ActionBlock<int>(i => ConsumerOne(i), nonGreedy);
         var consumeTwo = new ActionBlock<int>(i => ConsumerTwo(i), greedy);

         source.LinkTo(consumeOne);
         source.LinkTo(consumeTwo);

         for (var i = 0; i < 10; i++)
         {
            source.Post(i);
            Thread.Sleep(50);
         }

         Console.ReadLine();
      }
   }
}