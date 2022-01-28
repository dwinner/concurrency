/**
 * Некорректное потребление коллекций в поставщиках
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace UsingBlockingCollectionAsIEnum
{
   internal static class Program
   {
      private static void Main()
      {
         var blockingCollection = new BlockingCollection<int>();

         Task.Factory.StartNew(() =>
         {
            Thread.Sleep(500);
            for (int i = 0; i < 100; i++) blockingCollection.Add(i);
            blockingCollection.CompleteAdding();
         });

         Task consumerTask = Task.Factory.StartNew(() =>
         {
            // BUG: Здесь мы ничего не получим
            foreach (int i in blockingCollection) Console.WriteLine("Item {0}", i);
            Console.WriteLine("Collection is fully consumed");
         });

         consumerTask.Wait();
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}