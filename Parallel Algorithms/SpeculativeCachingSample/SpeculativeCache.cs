using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SpeculativeCachingSample
{
   public class SpeculativeCache<TKey, TValue>
   {
      private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary;
      private readonly Func<TKey, TValue> _factoryFunction;

      public SpeculativeCache(Func<TKey, TValue> factory, Func<TKey, TKey[]> speculator)
      {
         _dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(); // Инициализируем словарь
         var queue = new BlockingCollection<TKey>();

         _factoryFunction = key =>  // Создадим обертку
         {
            TValue value = factory(key);  // Вызовем фабричную функцию
            queue.Add(key);  // Добавим ключ в спекулятивную очередь            
            return value;  // Возвратим результат
         };

         // Стартуем задачу, которая отчечает на спекуляцию
         Task.Factory.StartNew(() =>
         {
            Parallel.ForEach(queue.GetConsumingEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 2 },
               key =>
               {
                  // Перечисляем ключи спекулятора
                  foreach (TKey specKey in speculator(key))
                  {
                     TKey localKey = specKey;
                     // ReSharper disable once UnusedVariable
                     TValue res = _dictionary.GetOrAdd(specKey, new Lazy<TValue>(() => factory(localKey))).Value;
                  }
               });
         });
      }

      public TValue GetValue(TKey key)
      {
         return _dictionary.GetOrAdd(key, new Lazy<TValue>(() => _factoryFunction(key))).Value;
      }
   }
}