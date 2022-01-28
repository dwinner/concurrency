/**
 * Инкапсуляция собственных блоков как один
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace CustomBlocks
{
   internal static class Program
   {
      private static void Main()
      {
         var options = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
         var propagatorBlock = CreateMyCustomBlock(options);
         var outputBlock = new ActionBlock<int>(i => { Console.Write("{0} ", i); });
         propagatorBlock.LinkTo(outputBlock);

         for (var i = 0; i < 10; i++)
         {
            propagatorBlock.Post(i);
         }

         Console.ReadLine();
      }

      private static IPropagatorBlock<int, int> CreateMyCustomBlock(ExecutionDataflowBlockOptions options)
      {
         var multiplyBlock = new TransformBlock<int, int>(item => item * 2, options);
         var addBlock = new TransformBlock<int, int>(item => item + 2, options);
         var divideBlock = new TransformBlock<int, int>(item => item / 2, options);

         var flowCompletion = new DataflowLinkOptions { PropagateCompletion = true };

         multiplyBlock.LinkTo(addBlock, flowCompletion);
         addBlock.LinkTo(divideBlock, flowCompletion);

         return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
      }
   }
}