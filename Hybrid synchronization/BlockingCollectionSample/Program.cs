/**
 * Блокирующие коллекции
 */

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace BlockingCollectionSample
{
   static class Program
   {
      public static void Main()
      {
         var blocking = new BlockingCollection<int>(new ConcurrentQueue<int>());

         // Поток пула получает элементы
         ThreadPool.QueueUserWorkItem(ConsumeItems, blocking);

         // Добавляем в коллекцию 5 элементов
         for (int item = 0; item < 5; item++)
         {
            Console.WriteLine("Producing: {0}", item);
            blocking.Add(item);
         }

         // Информируем поток-потребитель, что больше элементов не будет
         blocking.CompleteAdding();

         Console.ReadLine();  // Для целей тестирования
      }

      private static void ConsumeItems(object state)
      {
         var blockingCollection = state as BlockingCollection<int>;
         if (blockingCollection != null)
         {
            // Блокируем до получения элемента, затем обрабатываем его
            foreach (var item in blockingCollection.GetConsumingEnumerable())
            {
               Console.WriteLine("Consuming: {0}", item);
            }

            // Коллекция пуста и там больше не будет элементов
            Console.WriteLine("All items have been consumed: {0}", blockingCollection.IsCompleted ? "completed" : "error");
         }
      }
   }
}
