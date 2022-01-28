using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace SingleBlockCancellation
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         var slowAction = new ActionBlock<int>(i =>
         {
            Console.WriteLine("{0}:Started", i);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("{0}:Done", i);
         }, new ExecutionDataflowBlockOptions {CancellationToken = tokenSource.Token});

         slowAction.Post(1);
         slowAction.Post(2);
         slowAction.Post(3);

         slowAction.Complete();

         slowAction.Completion.ContinueWith(
            task => Console.WriteLine("Blocked finished in state of {0}", task.Status), tokenSource.Token);

         Console.ReadLine();
         tokenSource.Cancel();

         Console.ReadLine();
      }
   }
}