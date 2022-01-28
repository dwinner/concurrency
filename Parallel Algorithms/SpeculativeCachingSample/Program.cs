/**
 * Спекулятивный кэш
 */

using System;
using System.Linq;

namespace SpeculativeCachingSample
{
   static class Program
   {
      static void Main()
      {
         // Создадим новый экземпляр кэша
         var cache = new SpeculativeCache<int, double>(key1 =>
         {
            Console.WriteLine("Created value for key {0}", key1);
            return Math.Pow(key1, 2);
         },
         key2 => Enumerable.Range(key2 + 1, 5).ToArray());

         // Запросим нексколько значений из кэша
         for (int i = 0; i < 100; i++)
         {
            double value = cache.GetValue(i);
            Console.WriteLine("Got result {0} for key {1}", value, i);
         }

         // Ждем ввода до завершения
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
