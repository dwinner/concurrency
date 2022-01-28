/**
 * Остановка параллельных циклов
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _06_StoppingLoop
{
   internal static class Program
   {
      private static void Main()
      {
         var dataItems = new List<string>
         {
            "an",
            "apple",
            "a",
            "day",
            "keeps",
            "the",
            "doctor",
            "away"
         };

         Parallel.ForEach(dataItems, (item, state) =>
         {
            if (item.Contains("k"))
            {
               Console.WriteLine("Hit: {0}", item);
               state.Stop();
            }
            else
            {
               Console.WriteLine("Miss: {0}", item);
            }
         });

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}