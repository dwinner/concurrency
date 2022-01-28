/**
 * Прерывание параллельного цикла
 */

using System;
using System.Threading.Tasks;

namespace _07_BreakingLoop
{
   internal static class Program
   {
      private static void Main()
      {
         var loopResult = Parallel.For(0, 100, (index, state) =>
         {
            double sqr = Math.Pow(index, 2);
            if (sqr > 100)
            {
               Console.WriteLine("Breaking on index {0}", index);
               state.Break();
            }
            else
            {
               Console.WriteLine("Square value of {0} is {1}", index, sqr);
            }
         });

         Console.WriteLine("Lowest break iteration: {0}", loopResult.LowestBreakIteration);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}