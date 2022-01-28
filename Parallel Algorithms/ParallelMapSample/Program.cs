/**
 * Проекции в параллельном режиме
 */

using System;
using System.Linq;

namespace ParallelMapSample
{
   static class Program
   {
      static void Main()
      {
         // Создаем источник данных
         int[] sourceData = Enumerable.Range(0, 100).ToArray();

         // Определаем функцию для проекции
         Func<int, double> mapFunc = value => Math.Pow(value, 2);

         // Проецируем данные
         double[] resultData = ParallelMap.ParallelMapping(mapFunc, sourceData);

         // Выводим результаты
         for (int i = 0; i < sourceData.Length; i++)
         {
            Console.WriteLine("Value {0} mapped to {1}", sourceData[i], resultData[i]);
         }
      }
   }
}
