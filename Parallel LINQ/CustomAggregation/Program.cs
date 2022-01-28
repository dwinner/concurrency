/**
 * Параллельная агрегация в PLINQ
 */

using System;
using System.Linq;

namespace CustomAggregation
{
   internal static class Program
   {
      private static void Main()
      {
         var sourceData = new int[10000];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = i;
         }

         var result = sourceData.AsParallel().Aggregate(
            0.0, // Инициализация результата
            (subtotal, item) => subtotal + Math.Pow(item, 2), // Вычисление каждого элемента в отдельной задаче
            (total, subtotal) => total + subtotal, // Подсчет текущего итога
            total => total / 2 // Вычисление конечного результата
            );

         Console.WriteLine("Total: {0}", result);
      }
   }
}