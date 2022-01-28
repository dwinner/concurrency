using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace _16_OrderableContextPartitioner
{
   public class ContextPartitioner : OrderablePartitioner<WorkItem>
   {
      private long _sharedStartIndex; // Первый неразбитый индекс
      private readonly WorkItem[] _dataItems; // Набор данных для разбиения
      private readonly EnumerableSource _enumSource;
      private readonly object _lockObj = new object(); // Объект блокировки для предотвращения гонок в индексах
      private readonly int _targetSum; // сумма значений для каждого разбиения

      public ContextPartitioner(WorkItem[] data, int target)
         : base(true, false, true)
      {
         _dataItems = data;
         _targetSum = target;
         _enumSource = new EnumerableSource(this);
      }

      public override bool SupportsDynamicPartitions
      {
         get { return true; }
      }

      public override IList<IEnumerator<KeyValuePair<long, WorkItem>>> GetOrderablePartitions(int partitionCount)
      {
         IList<IEnumerator<KeyValuePair<long, WorkItem>>> partitionsList =
            new List<IEnumerator<KeyValuePair<long, WorkItem>>>();

         // Получаем перечислитель, который будет генерировать динамические разбиения
         var enumObj = GetOrderableDynamicPartitions();

         // Создаем требуемое число разбиений
         for (var i = 0; i < partitionCount; i++)
         {
            // ReSharper disable PossibleMultipleEnumeration
            partitionsList.Add(enumObj.GetEnumerator());
            // ReSharper restore PossibleMultipleEnumeration
         }

         return partitionsList;
      }

      public override IEnumerable<KeyValuePair<long, WorkItem>> GetOrderableDynamicPartitions()
      {
         return _enumSource;
      }

      private Tuple<long, long> GetNextChunk()
      {
         Tuple<long, long> result;
         lock (_lockObj)
         {
            // Проверяем, есть еще данные для разбиения по блокам
            if (_sharedStartIndex < _dataItems.Length)
            {
               var sum = 0;
               var endIndex = _sharedStartIndex;
               while (endIndex < _dataItems.Length && sum < _targetSum)
               {
                  sum += _dataItems[endIndex].WorkDuraction;
                  endIndex++;
               }

               result = Tuple.Create(_sharedStartIndex, endIndex);
               _sharedStartIndex = endIndex;
            }
            else
            {
               result = Tuple.Create(0L, 0L);
            }
         }

         return result;
      }

      private class EnumerableSource : IEnumerable<KeyValuePair<long, WorkItem>>
      {
         private readonly ContextPartitioner _parentPartitioner;

         public EnumerableSource(ContextPartitioner parentPartitioner)
         {
            _parentPartitioner = parentPartitioner;
         }

         public IEnumerator<KeyValuePair<long, WorkItem>> GetEnumerator()
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

         public IEnumerator<KeyValuePair<long, WorkItem>> GetEnumerator()
         {
            while (true)
            {
               // Получаем индексы следующего разбиения
               var chunkIndeces = _parentPartitioner.GetNextChunk();

               // Проверяем, что у нас есть данные для разбиения
               if (chunkIndeces.Item1 == -1 && chunkIndeces.Item2 == -1)
               {
                  yield break;
               }

               for (var i = chunkIndeces.Item1; i < chunkIndeces.Item2; i++)
               {
                  yield return new KeyValuePair<long, WorkItem>(i, _parentPartitioner._dataItems[i]);
               }
            }
         }
      }
   }
}