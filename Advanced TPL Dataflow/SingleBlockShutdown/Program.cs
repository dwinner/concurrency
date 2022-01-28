using System;
using System.Threading.Tasks.Dataflow;

namespace SingleBlockShutdown
{
   internal static class Program
   {
      private static void Main()
      {
         var actionBlock = new ActionBlock<int>(i => Console.WriteLine(i));
         for (var i = 0; i < 10; i++)
            actionBlock.Post(i);

         Console.WriteLine("Completing...");
         actionBlock.Complete();
         Console.WriteLine("Waiting...");
         actionBlock.Completion.Wait();
         Console.WriteLine(actionBlock.Completion.Status);
      }
   }
}