/**
 * Параллельный цикл For
 */

using System;
using System.Threading.Tasks;

namespace _01_ParallelForLoop
{
   internal static class Program
   {
      private static void Main()
      {
         var dataItems = new int[100];
         var resultItems = new double[100];

         for (int i = 0; i < dataItems.Length; i++)
         {
            dataItems[i] = i;
         }

         Parallel.For(0, dataItems.Length, (index, loopState) => { resultItems[index] = Math.Pow(dataItems[index], 2); });

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}