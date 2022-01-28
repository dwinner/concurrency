/**
 * Развязывание ссылок
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace UnlinkingBlocks
{
   internal static class Program
   {
      private static void Main()
      {
         CompletionBlockSample();
         Console.ReadKey();
      }

      private static async void CompletionBlockSample()
      {
         var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
         var subtractBlock = new TransformBlock<int, int>(item => item - 2);
         var options = new DataflowLinkOptions { PropagateCompletion = true };
         using (multiplyBlock.LinkTo(subtractBlock, options))
         {
            // Завершение первого блока автоматически распространяется на второй
            multiplyBlock.Complete();
            await subtractBlock.Completion;
         }
      }
   }
}