/**
 * Асинхронные оболочки для параллельных контейнеров
 */

using System.Collections.Concurrent;

namespace AsynchStacksAndBags
{
   internal static class Program
   {
      private static void Main()
      {
         var stackWrapper = new AsyncCollectionSample(new ConcurrentStack<int>());
         var bagWrapper = new AsyncCollectionSample(new ConcurrentBag<int>());
         var throttleQueueWrapper = new AsyncCollectionSample(new ConcurrentQueue<int>(), 1);

         stackWrapper.Go();
         bagWrapper.Go();
         throttleQueueWrapper.Go();
      }
   }
}