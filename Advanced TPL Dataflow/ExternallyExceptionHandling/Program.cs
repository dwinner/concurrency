using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.Console;

namespace ExternallyExceptionHandling
{
   internal static class Program
   {
      private static void Main()
      {
         var divideBlock = new ActionBlock<(int, int)>(
            tuple => { WriteLine(tuple.Item1 / tuple.Item2); }
            , new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 4});

         divideBlock.Post((10, 5));
         divideBlock.Post((20, 0));
         divideBlock.Post((10, 0));
         divideBlock.Post((10, 2));

         divideBlock.Completion.ContinueWith(task =>
         {
            var innerExceptions = task.Exception?.Flatten().InnerExceptions;
            if (innerExceptions != null)
               foreach (var innerException in innerExceptions)
                  WriteLine("Divide block failed Reason: {0}", innerException.Message);
         }, TaskContinuationOptions.OnlyOnFaulted);

         ReadLine();
      }
   }
}