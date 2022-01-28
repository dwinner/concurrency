/**
 * Проблема переиспользования объектов в поставщике
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReusingObjectsInProducers
{
   internal static class Program
   {
      private static void Main()
      {
         var blockingCollection = new BlockingCollection<DataItem>();

         // Создаем и запускаем потребителя
         Task consumerTask = Task.Factory.StartNew(() =>
         {
            while (!blockingCollection.IsCompleted)
            {
               DataItem item;
               if (blockingCollection.TryTake(out item))
               {
                  Console.WriteLine("Item counter {0}", item.Counter);
               }
            }
         });

         // Создаем и запускаем производителя
         Task.Factory.StartNew(() =>
         {
            // BUG - Reusing: var item = new DataItem();
            for (int i = 0; i < 100; i++)
            {
               var item = new DataItem { Counter = i };
               blockingCollection.Add(item);
            }

            blockingCollection.CompleteAdding();
         });

         consumerTask.Wait();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class DataItem
   {
      public int Counter { get; set; }
   }
}