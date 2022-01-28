using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace JoinInPairs
{
   internal static class Program
   {
      private static void Main()
      {
         var broadcastBlock = new BroadcastBlock<int>(i => i);

         var bufferOne = new BufferBlock<int>();
         var bufferTwo = new BufferBlock<int>();

         var firstJoinBlock = new JoinBlock<int, int>(new GroupingDataflowBlockOptions {Greedy = false});

         var consumer = new ActionBlock<Tuple<int, int>>(tuple =>
         {
            if (tuple.Item1 != tuple.Item2)
               Console.WriteLine(tuple);
         });

         bufferOne.LinkTo(firstJoinBlock.Target1);
         bufferTwo.LinkTo(firstJoinBlock.Target2);

         firstJoinBlock.LinkTo(consumer);

         broadcastBlock.LinkTo(bufferOne);
         broadcastBlock.LinkTo(bufferTwo);

         for (var nTask = 0; nTask < 4; nTask++)
         {
            var localTask = nTask;
            Task.Factory.StartNew(() =>
            {
               while (true)
                  broadcastBlock.Post(localTask);

               // Could cause join to pair incorrectly
               //bufferOne.Post(localTask);
               //bufferTwo.Post(localTask);
               // ReSharper disable FunctionNeverReturns
            });
            // ReSharper restore FunctionNeverReturns
         }

         Console.ReadLine();
      }
   }
}