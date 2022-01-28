using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace FirstAttempt
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         var divideBlock = new TransformBlock<Tuple<int, int>, int>(pair => pair.Item1 / pair.Item2,
            new ExecutionDataflowBlockOptions {CancellationToken = tokenSource.Token});
         var divideConsumer = new ActionBlock<int>(result =>
         {
            Console.WriteLine(result);
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
         }, new ExecutionDataflowBlockOptions {BoundedCapacity = 11, CancellationToken = tokenSource.Token});

         divideBlock.LinkTo(divideConsumer, new DataflowLinkOptions {PropagateCompletion = true});
         divideBlock.Completion.ContinueWith(dbt => divideConsumer.Complete(), tokenSource.Token);

         var random = new Random();
         tokenSource.CancelAfter(TimeSpan.FromSeconds(2));

         for (var i = 0; i < 10; i++)
            divideBlock.Post(Tuple.Create(random.Next(1, 10), random.Next(1, 10)));

         divideBlock.Complete();
         divideBlock.Completion.Wait(tokenSource.Token);
         Console.WriteLine("Posting done");
         divideConsumer.Completion.Wait(tokenSource.Token);
      }
   }
}