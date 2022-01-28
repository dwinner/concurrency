using System;
using System.Threading.Tasks.Dataflow;
using static System.Console;

namespace PropagatingAndHandlingExceptions
{
   internal static class Program
   {
      private static void Main()
      {
         var divideBlock = new TransformBlock<(int, int), int>(tuple => tuple.Item1 / tuple.Item2);
         var printingBlock = new ActionBlock<int>(i => WriteLine(i));

         divideBlock.LinkTo(printingBlock, new DataflowLinkOptions {PropagateCompletion = true});

         divideBlock.Post((10, 5));
         divideBlock.Post((20, 4));
         divideBlock.Post((10, 0));
         divideBlock.Post((10, 2));

         divideBlock.Complete();

         try
         {
            printingBlock.Completion.Wait();
         }
         catch (AggregateException aggrEx)
         {
            foreach (var innerError in aggrEx.Flatten().InnerExceptions)
               WriteLine("Divide block failed reason: {0}", innerError.Message);
         }
      }
   }
}