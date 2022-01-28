using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace LinkedCancellation
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();
         var slowTransform = new TransformBlock<int, string>(
            i =>
            {
               Console.WriteLine("{0}:Started", i);
               tokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));
               tokenSource.Token.ThrowIfCancellationRequested();
               Console.WriteLine("{0}:Done", i);

               return i.ToString();
            }, new ExecutionDataflowBlockOptions {CancellationToken = tokenSource.Token});

         var printAction = new ActionBlock<string>(s => Console.WriteLine(s),
            new ExecutionDataflowBlockOptions {CancellationToken = tokenSource.Token});

         slowTransform.LinkTo(printAction, new DataflowLinkOptions {PropagateCompletion = true});

         slowTransform.Post(1);
         slowTransform.Post(2);
         slowTransform.Post(3);
         
         Console.ReadLine();
         tokenSource.Cancel();

         printAction.Completion.ContinueWith(pat => Console.WriteLine(pat.Status), tokenSource.Token);

         Console.ReadLine();
      }
   }
}