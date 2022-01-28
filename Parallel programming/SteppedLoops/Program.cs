/**
 * Шаговый итератор при параллельном обходе
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _04_SteppedLoops
{
   internal static class Program
   {
      private static void Main()
      {
         Parallel.ForEach(SteppedIterator(0, 10, 2), index => Console.WriteLine("Index value: {0}", index));

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }

      private static IEnumerable<int> SteppedIterator(int startIndex, int endIndex, int stepSize)
      {
         for (int i = startIndex; i < endIndex; i += stepSize)
         {
            yield return i;
         }
      }
   }
}