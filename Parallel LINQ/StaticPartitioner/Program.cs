/**
 * Статическое разбиение
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace StaticPartitioner
{
   internal static class Program
   {
      private static void Main()
      {
         var sourceData = new int[10];
         for (var i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = i;
         }

         var partitioner = new StaticPartitioner<int>(sourceData);
         IEnumerable<double> results = partitioner.AsParallel().Select(item => Math.Pow(item, 2));

         foreach (var d in results)
         {
            Console.WriteLine("Enumeration got result {0}", d);
         }
      }
   }

   internal class StaticPartitioner<T> : Partitioner<T>
   {
      private readonly T[] _data;

      public StaticPartitioner(T[] data)
      {
         _data = data;
      }

      public override bool SupportsDynamicPartitions
      {
         get { return false; }
      }

      public override IList<IEnumerator<T>> GetPartitions(int partitionCount)
      {
         IList<IEnumerator<T>> list = new List<IEnumerator<T>>(); // создаем список
         var itemsPerEnum = _data.Length / partitionCount; // Определяем, сколько элементов должно быть обработано за раз

         // Разбиваем
         for (var i = 0; i < partitionCount - 1; i++)
            list.Add(CreateEnum(i * itemsPerEnum, (i + 1) * itemsPerEnum));
         list.Add(CreateEnum((partitionCount - 1) * itemsPerEnum, _data.Length));

         return list;
      }

      private IEnumerator<T> CreateEnum(int startIndex, int endIndex)
      {
         var index = startIndex;
         while (index < endIndex)
         {
            yield return _data[index++];
         }
      }
   }
}