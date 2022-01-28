using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ErrorsApp
{
   internal static class Program
   {
      private static void Main()
      {
         var errorBlock = new TransformBlock<int, int>((Func<int, int>) Do,
            new ExecutionDataflowBlockOptions {BoundedCapacity = 4, MaxMessagesPerTask = 1});
         var nonErrorBlock = new TransformBlock<int, int>(i => i);
         var consumeBlock = new ActionBlock<int>(i => Console.WriteLine(i));

         errorBlock.LinkTo(consumeBlock);
         nonErrorBlock.LinkTo(consumeBlock);

         for (var i = 1; i < 10; i++)
         {
            //var postFailed = errorBlock.Post(i);
            nonErrorBlock.Post(i);
            if (errorBlock.Completion.IsFaulted)
            {
               Console.WriteLine("Faulted");
               ((IDataflowBlock) consumeBlock).Fault(errorBlock.Completion.Exception);
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
         }

         Console.ReadLine();
         consumeBlock.Complete();

         try
         {
            consumeBlock.Completion.Wait();
         }
         catch (AggregateException errors)
         {
            foreach (var error in errors.Flatten().InnerExceptions)
               Console.WriteLine(error.Message);
         }
      }

      private static int Do(int arg)
      {
         Console.WriteLine(Task.CurrentId);
         if (arg % 2 == 0)
         {
            throw new Exception("Boom");
         }

         return arg;
      }
   }
}