using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace _15_ContextualPartitioner
{
   public class ContextPartitioner : Partitioner<WorkItem>
   {
      private readonly WorkItem[] _dataItems; // Набор данных для разбиения
      private readonly int _targetSum; // Сумма значений для одного разбиения
      private readonly EnumerableSource _enumSource; // Объект для создания перечислителей
      private readonly object _lockObj = new object(); // Для предотвращения гонок в индексах
      private const long SharedStartIndex = 0; // Первый "не разбитый" элемент

      public ContextPartitioner(WorkItem[] data, int target)
      {
         _dataItems = data;
         _targetSum = target;
         _enumSource = new EnumerableSource(this);
      }

      // NOTE: Динамическое разбиение должно быть доступно для параллельных циклов Foreach
      public override bool SupportsDynamicPartitions { get { return true; } }

      public override IEnumerable<WorkItem> GetDynamicPartitions()
      {
         return _enumSource;
      }

      public override IList<IEnumerator<WorkItem>> GetPartitions(int partitionCount)
      {
         IList<IEnumerator<WorkItem>> partitionsList = new List<IEnumerator<WorkItem>>();
         var enumObj = GetDynamicPartitions(); // Получаем перечислитель, который генерирует разбиения динамически
         // Создаем необходимое кол-во разбиений
         for (var i = 0; i < partitionCount; i++)
         {
            // ReSharper disable PossibleMultipleEnumeration
            partitionsList.Add(enumObj.GetEnumerator());
            // ReSharper restore PossibleMultipleEnumeration
         }

         return partitionsList;
      }

      private Tuple<long, long> GetNextChunk()
      {
         Tuple<long, long> result;
         lock (_lockObj)
         {
            if (SharedStartIndex < _dataItems.Length) // Проверим, есть еще элементы для разбиения
            {
               var sum = 0;
               var endIndex = SharedStartIndex;
               while (endIndex < _dataItems.Length && sum < _targetSum)
               {
                  sum += _dataItems[endIndex++].WorkDuration;
               }

               result = Tuple.Create(SharedStartIndex, endIndex);
            }
            else // Данных для разбиения больше не осталось
            {
               result = Tuple.Create(-1L, -1L);
            }
         }

         return result;
      }

      private class EnumerableSource : IEnumerable<WorkItem>
      {
         private readonly ContextPartitioner _parentPartitioner;

         public EnumerableSource(ContextPartitioner parentPartitioner)
         {
            _parentPartitioner = parentPartitioner;
         }

         public IEnumerator<WorkItem> GetEnumerator()
         {
            return new ChunkEnumerator(_parentPartitioner).GetEnumerator();
         }

         IEnumerator IEnumerable.GetEnumerator()
         {
            return GetEnumerator();
         }
      }

      private class ChunkEnumerator
      {
         private readonly ContextPartitioner _parentPartitioner;

         public ChunkEnumerator(ContextPartitioner parentPartitioner)
         {
            _parentPartitioner = parentPartitioner;
         }

         public IEnumerator<WorkItem> GetEnumerator()
         {
            while (true)
            {
               var nextChunk = _parentPartitioner.GetNextChunk(); // Получаем индексы для следующего разбиения
               if (nextChunk.Item1 == -1 && nextChunk.Item2 == -1)
               {
                  yield break;
               }

               for (var i = nextChunk.Item1; i < nextChunk.Item2; i++)
               {
                  yield return _parentPartitioner._dataItems[i];
               }
            }
         }
      }
   }
}