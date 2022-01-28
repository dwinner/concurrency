using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelQuickSortSample
{
   public static class ParallelSort<T>
   {
      public const int DefaultMaxDepth = 16;
      public const int DefaultMinBlockSize = 10000;

      public static void ParallelQuickSort(T[] data, IComparer<T> comparer, int maxDepth = DefaultMaxDepth, int minBlockSize = DefaultMinBlockSize)
      {
         InternalSort(data, 0, data.Length - 1, comparer, 0, maxDepth, minBlockSize);
      }

      private static void InternalSort(T[] data, int startIndex, int endIndex, IComparer<T> comparer, int depth, int maxDepth, int minBlockSize)
      {
         if (startIndex < endIndex)
         {
            // Если мы превысили пороговое значение или у нас есть слишком мало
            // значений в контейнере, чем нам нужно, тогда используем последовательную сортировку
            if (depth > maxDepth || endIndex - startIndex < minBlockSize)
            {
               Array.Sort(data, startIndex, endIndex - startIndex + 1, comparer);
            }
            else
            {
               // Нам нужна параллельность
               int pivotIndex = PartitionBlock(data, startIndex, endIndex, comparer);

               // Рекурсивно разделяем на левый и правый блоки
               Task leftTask =
                  Task.Factory.StartNew(
                     () => InternalSort(data, startIndex, pivotIndex - 1, comparer, depth + 1, maxDepth, minBlockSize));
               Task rightTask =
                  Task.Factory.StartNew(
                     () => InternalSort(data, pivotIndex + 1, endIndex, comparer, depth + 1, maxDepth, minBlockSize));

               // Ждем завершения всех задач
               Task.WaitAll(leftTask, rightTask);
            }
         }
      }

      internal static int PartitionBlock(T[] data, int startIndex, int endIndex, IComparer<T> comparer)
      {
         // Получаем пороговое значение для разделения, мы будем сравнивать
         // элементы, разделяя блоки, основываясь на этом значении
         T pivot = data[startIndex];

         // Сохряняем пороговое значение в конце блока
         SwapValues(data, startIndex, endIndex);

         // Индекс нужен для сохранения значений, которые меньше чем пороговое
         int storeIndex = startIndex;

         // Проходим через элементы в блоке
         for (int i = startIndex; i < endIndex; i++)
         {
            // Ищем элементы, которые меньше или равны пороговому значению
            if (comparer.Compare(data[i], pivot) < 0)
            {
               // Передвигаем элемент и увеличиваем значение
               SwapValues(data, i, storeIndex);
               storeIndex++;
            }
         }
         SwapValues(data, storeIndex, endIndex);

         return storeIndex;
      }

      internal static void SwapValues(T[] data, int firstIndex, int secondIndex)
      {
         T holder = data[firstIndex];
         data[firstIndex] = data[secondIndex];
         data[secondIndex] = holder;
      }
   }
}