/**
 * Выполнение запроса без получения результата
 */

using System;
using System.Linq;

namespace NoResultQuery
{
   internal static class Program
   {
      private static void Main()
      {
         var sourceData = new int[50];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = i;
         }

         // Фильтруем данные и вызываем ForAll()
         sourceData.AsParallel()
            .Where(item => item % 2 == 0)
            .ForAll(item => Console.WriteLine("Item {0} Result {1}", item, Math.Pow(item, 2)));
      }
   }
}