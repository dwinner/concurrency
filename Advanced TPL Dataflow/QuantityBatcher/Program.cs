using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace QuantityBatcher
{
   internal static class Program
   {
      private static void Main()
      {
         const int batchSize = 100;
         var batcher = new BatchBlock<int>(batchSize);

         var averager = new ActionBlock<int[]>(intValues =>
            Console.WriteLine("{0} reduced to {1}", intValues.Length, intValues.Average()));

         batcher.LinkTo(averager);

         Console.WriteLine("Press 'Q' to quit");
         var rnd = new Random();
         bool quit;
         do
         {
            Thread.Sleep(TimeSpan.FromMilliseconds(10));
            batcher.Post(rnd.Next(1, 100));
            quit = Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q;
         } while (!quit);

         batcher.Complete();
         batcher.Completion.Wait();
      }
   }
}