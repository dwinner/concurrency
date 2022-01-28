using System;
using System.Threading.Tasks.Dataflow;

namespace SimpleJoinSample
{
   internal static class Program
   {
      private static void Main()
      {
         var bufferOne = new BufferBlock<int>();
         var bufferTwo = new BufferBlock<int>();

         var firstJoinBlock = new JoinBlock<int, int>(new GroupingDataflowBlockOptions {Greedy = false});

         var consumer = new ActionBlock<Tuple<int, int>>(tuple => Console.WriteLine(tuple));

         bufferOne.LinkTo(firstJoinBlock.Target1);
         bufferTwo.LinkTo(firstJoinBlock.Target2);

         firstJoinBlock.LinkTo(consumer);

         bufferOne.Post(1);
         bufferTwo.Post(1);

         Console.ReadLine();
      }
   }
}