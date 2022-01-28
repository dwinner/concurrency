using System;
using System.Linq;

namespace ConfusingOrdering
{
   internal static class Program
   {
      private static void Main()
      {
         string[] sourceData =
         {
            "an", "apple", "a", "day", "keeps", "the", "doctor", "away"
         };

         // Сохранение порядка в источнике
         var result1 = sourceData.AsParallel().AsOrdered().Select(item => item);
         foreach (var v in result1)
         {
            Console.WriteLine("AsOrdered() - {0}", v);
         }

         // Сортировка обработанных результатов
         var result2 = sourceData.AsParallel().OrderBy(item => item).Select(item => item);
         foreach (var v in result2)
         {
            Console.WriteLine("OrderBy() - {0}", v);
         }
      }
   }
}