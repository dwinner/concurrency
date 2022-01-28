/**
 * Параллельная обработка данных
 */


using System.Threading.Tasks.Dataflow;

namespace ParallelProcessing
{
   internal static class Program
   {
      private static void Main()
      {
         var multiplyBlock = new TransformBlock<int, int>(
            item => item * 2,
            new ExecutionDataflowBlockOptions
            {
               MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            });
         var subtractBlock = new TransformBlock<int, int>(item => item - 2);
         multiplyBlock.LinkTo(subtractBlock);
      }
   }
}