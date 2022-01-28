/**
 * Разбиение с сохранением порядка обработки данных
 */

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace _16_OrderableContextPartitioner
{
   internal static class Program
   {
      private static void Main()
      {
         var rnd = new Random();

         var sourceData = new WorkItem[10000];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = new WorkItem
            {
               WorkDuraction = rnd.Next(1, 11)
            };
         }

         var resultData = new WorkItem[sourceData.Length];

         OrderablePartitioner<WorkItem> cPartitioner = new ContextPartitioner(sourceData, 100);

         Parallel.ForEach(cPartitioner, (item, state, idx) =>
         {
            item.PerformWork();
            resultData[idx] = item;
         });

         if (sourceData.Where((workItem, index) => workItem.WorkDuraction != resultData[index].WorkDuraction).Any())
         {
            Console.WriteLine("Discrepancy at index {0}");
         }
      }
   }
}