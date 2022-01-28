/**
 * Разбиение с учетом порядка
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _14_OrderablePartitioner
{
   internal static class Program
   {
      private static void Main()
      {
         IList<string> sourceData = new List<string>
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

         var resultData = new string[sourceData.Count];

         var op = Partitioner.Create(sourceData);

         Parallel.ForEach(op, (item, loopState, index) =>
         {
            if (item == "apple")
            {
               item = "apricot";
            }

            resultData[index] = item;
         });

         for (var i = 0; i < resultData.Length; i++)
         {
            Console.WriteLine("Item {0} is {1}", i, resultData[i]);
         }
      }
   }
}