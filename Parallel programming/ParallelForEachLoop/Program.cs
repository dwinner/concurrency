/**
 * Параллельные циклы на итераторах
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _03_ParallelForEachLoop
{
   internal static class Program
   {
      private static void Main()
      {
         var dataList = new List<string>
         {
            "the",
            "quick",
            "brown",
            "fox",
            "jumps",
            "etc"
         };

         Parallel.ForEach(dataList, item => Console.WriteLine("Item {0} has {1} characters", item, item.Length));

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}