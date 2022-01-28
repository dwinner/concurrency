/**
 * Балансировка нагрузки на блоки
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace ThrottlingBlocks
{
   internal static class Program
   {
      private static void Main()
      {
         var sourceBlock = new BufferBlock<int>();
         var options = new DataflowBlockOptions { BoundedCapacity = 1 };

         var targetBlockA = new BufferBlock<int>(options);
         var targetBlockB = new BufferBlock<int>(options);
         var outputBlock = new ActionBlock<int>(i => { Console.WriteLine(i); });

         sourceBlock.LinkTo(targetBlockA);
         sourceBlock.LinkTo(targetBlockB);

         targetBlockA.LinkTo(outputBlock);
         targetBlockB.LinkTo(outputBlock);

         for (var i = 0; i < 10; i++)
         {
            sourceBlock.Post(i);
         }

         Console.ReadLine();
      }
   }
}