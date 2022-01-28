/**
 * Собственная реализация стратегий разбиения в параллельных циклах
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _15_ContextualPartitioner
{
   internal static class Program
   {
      private static void Main()
      {
         var rnd = new Random();

         var sourceData = new WorkItem[10000];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = new WorkItem(rnd.Next(1, 11));
         }

         // Создаем стратегию разбиения для конкретного типа
         Partitioner<WorkItem> contextualPartitioner = new ContextPartitioner(sourceData, 100);

         Parallel.ForEach(contextualPartitioner, item => { item.PerformWork(); });
      }
   }
}