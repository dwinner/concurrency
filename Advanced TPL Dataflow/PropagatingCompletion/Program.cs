using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace PropagatingCompletion
{
   internal static class Program
   {
      private static void Main()
      {
         var firstBlock = new TransformBlock<int, int>(i => i * 2);
         var secondBlock = new ActionBlock<int>(i =>
         {
            Thread.Sleep(TimeSpan.FromSeconds(.5));
            Console.WriteLine(i);
         });

         var thirdBlock = new ActionBlock<int>(i => Console.WriteLine(i));

         firstBlock.LinkTo(secondBlock, new DataflowLinkOptions {PropagateCompletion = true});
         firstBlock.LinkTo(thirdBlock, new DataflowLinkOptions {PropagateCompletion = true});

         for (var i = 0; i < 10; i++)
            firstBlock.Post(i);

         firstBlock.Complete();
         secondBlock.Completion.Wait();
      }
   }
}