using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace BoundedTransformBlock
{
   internal static class Program
   {
      private static void Main()
      {
         var divideBlock = new TransformBlock<Tuple<int, int>, int>(pair => pair.Item1 / pair.Item2);
         var divideConsumer = new ActionBlock<int>(i =>
         {
            Console.WriteLine(i);
            Thread.Sleep(TimeSpan.FromSeconds(1));
         }, new ExecutionDataflowBlockOptions {BoundedCapacity = 1});

         divideBlock.LinkTo(divideConsumer, new DataflowLinkOptions {PropagateCompletion = true});
         divideConsumer.Completion.ContinueWith(task => divideConsumer.Complete());

         var random = new Random();
         for (var i = 0; i < 10; i++)
            divideBlock.Post(Tuple.Create(random.Next(1, 100), random.Next(1, 10)));

         divideBlock.Complete();
         divideBlock.Completion.Wait();
         Console.WriteLine("Posting done");
         divideConsumer.Completion.Wait();
      }
   }
}