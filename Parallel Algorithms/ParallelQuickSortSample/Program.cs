/**
 * Параллельная сортировка
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace ParallelQuickSortSample
{
   static class Program
   {
      static void Main()
      {
         // Генерируем несколько случайных значений
         var random = new Random();
         var sourceData = new int[5000000];
         for (int i = 0; i < sourceData.Length; i++)
         {
            sourceData[i] = random.Next(1, 100);
         }

         // Сортируем массив параллельно
         ParallelSort<int>.ParallelQuickSort(sourceData, Comparer<int>.Default);
         Console.SetOut(new StreamWriter("Out.log"));
         foreach (var i in sourceData)
         {
            Console.WriteLine(i);
         }
      }
   }
}
