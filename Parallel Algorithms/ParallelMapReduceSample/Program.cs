/**
 * Редукция и проекции контейнеров в параллельном режиме
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace ParallelMapReduceSample
{
   static class Program
   {
      static void Main()
      {
         // создать функцию, в которой перечислены факторы
         Func<int, IEnumerable<int>> map = value =>
         {
            IList<int> factors = new List<int>();
            for (int i = 1; i < value; i++)
            {
               if (value % i == 0)
               {
                  factors.Add(i);
               }
            }

            return factors;
         };

         // создать групповую функцию - в этом примере мы хотим группировать
         // одни и те же результаты вместе, поэтому мы проецируем значение в себя же
         Func<int, int> group = value => value;

         // создать функцию снижения - это просто подсчитывает количество элементов
         // в группе и возвращает пару ключ/значение с результатом в
         // качестве ключа и количеством в качестве значения
         Func<IGrouping<int, int>, KeyValuePair<int, int>> reduce =
            grouping => new KeyValuePair<int, int>(grouping.Key, grouping.Count());

         // Возьмем исходные данные
         IEnumerable<int> sourceData = Enumerable.Range(1, 50);

         IEnumerable<KeyValuePair<int, int>> result = ParallelMapReduction.MapReduce(sourceData, map, @group, reduce);

         foreach (KeyValuePair<int, int> pair in result)
         {
            Console.WriteLine("{0} is a factor {1} times", pair.Key, pair.Value);
         }
      }
   }
}
