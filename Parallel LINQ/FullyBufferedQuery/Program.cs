/**
 * Полная буферизация результатов запроса
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace FullyBufferedQuery
{
   internal static class Program
   {
      private static void Main()
      {
         var sourceData = new int[5];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = i;
         }

         // Определяем буферизованный запрос
         IEnumerable<double> results = sourceData.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .Select(
               item =>
               {
                  var resultItem = Math.Pow(item, 2);
                  Console.WriteLine("Produced result {0}", resultItem);
                  return resultItem;
               });

         // Итерируем по результатам
         foreach (var d in results)
         {
            Console.WriteLine("Enumeration got result {0}", d);
         }
      }
   }
}