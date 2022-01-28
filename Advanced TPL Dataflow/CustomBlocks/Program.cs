using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

// ReSharper disable FunctionNeverReturns

namespace CustomBlocks
{
   internal static class Program
   {
      private static void Main()
      {
         var bufferBlock = new BufferBlock<int>();
         var tap = new Tap<int>();
         tap.LinkTo(new ActionBlock<int>(i =>
         {
            Console.WriteLine("Tap sent me {0}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
         }));

         bufferBlock.LinkTo(tap);
         bufferBlock.LinkTo(new ActionBlock<int>(i => Console.WriteLine(i)));

         while (true)
         {
            var postResult = bufferBlock.Post(1);
            Console.WriteLine(postResult);
            Console.ReadLine();
            tap.IsOpen = !tap.IsOpen;
         }
      }
   }
}