using System;
using System.Linq;

namespace ParallelMapSample
{
   public static class ParallelMap
   {
      /// <summary>
      /// Проецирование в параллельном режиме
      /// </summary>
      /// <typeparam name="TInput">Исходный тип данных</typeparam>
      /// <typeparam name="TOutput">Проецируемый тип данных</typeparam>
      /// <param name="mapFunction">Функция проекции</param>
      /// <param name="input">Исходная последовательность входных данных</param>
      /// <returns>Проецированная последовательность</returns>
      public static TOutput[] ParallelMapping<TInput, TOutput>(Func<TInput, TOutput> mapFunction, TInput[] input)
      {
         return input
            .AsParallel()
            .AsOrdered()
            .Select(mapFunction)
            .ToArray();
      }
   }
}