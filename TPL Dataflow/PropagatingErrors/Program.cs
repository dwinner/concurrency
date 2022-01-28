/**
 * Распространение ошибок между узлами потока данных
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace PropagatingErrors
{
   internal static class Program
   {
      private static void Main()
      {
         HandleErrors();
         Console.ReadKey();
      }

      private static async void HandleErrors()
      {
         try
         {
            var multiplyBlock = new TransformBlock<int, int>(item =>
            {
               if (item == 1)
                  throw new InvalidOperationException("Blech.");

               return item * 2;
            });
            var subtractBlock = new TransformBlock<int, int>(item => item - 2);
            multiplyBlock.LinkTo(subtractBlock, new DataflowLinkOptions { PropagateCompletion = true });
            multiplyBlock.Post(1);
            multiplyBlock.Post(2);
            await subtractBlock.Completion;
         }
         catch (AggregateException aggrEx)
         {
            var inners = aggrEx.InnerExceptions;
            if (inners != null && inners.Count > 0)
            {
               foreach (var innerEx in inners)
               {
                  Console.WriteLine(innerEx.Message);
               }
            }
         }
      }
   }
}