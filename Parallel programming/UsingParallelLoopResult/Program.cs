/**
 * Структура ParallelLoopResult
 */

using System;
using System.Threading.Tasks;

namespace _08_UsingParallelLoopResult
{
   internal static class Program
   {
      private static void Main(string[] args)
      {
         if (args == null) throw new ArgumentNullException("args");
         var loopResult = Parallel.For(0, 10, (index, loopState) =>
         {
            if (index == 2)
            {
               loopState.Stop();
            }
         });

         Console.WriteLine("Loop Result");
         Console.WriteLine("IsCompleted: {0}", loopResult.IsCompleted);
         Console.WriteLine("BreakValue: {0}", loopResult.LowestBreakIteration.HasValue);
      }
   }
}