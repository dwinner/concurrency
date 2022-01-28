/**
 * Разделение данных по сегментам, работающем в одном потоке
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _13_ChunkingPartitioner
{
   internal static class Program
   {
      private static void Main()
      {
         var resultData = new double[10000000];

         OrderablePartitioner<Tuple<int, int>> chunkPart = Partitioner.Create(0, resultData.Length, 10000);

         Parallel.ForEach(chunkPart, chunkRange =>
         {
            for (var i = chunkRange.Item1; i < chunkRange.Item2; i++)
            {
               resultData[i] = Math.Pow(i, 2);
            }
         });
      }
   }
}