/**
 * Потенциальная гонка
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TryingToTakeConcurrently
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем блокирующую коллекцию
         var blockingCollection = new BlockingCollection<int>();

         // Создаем и запускаем задачу-производителя
         Task.Factory.StartNew(() =>
         {
            for (int i = 0; i < 100000; i++) blockingCollection.Add(i);
            blockingCollection.CompleteAdding();
         });

         // Создаем и запускаем задачу-потребителя
         Task.Factory.StartNew(() =>
         {
            while (!blockingCollection.IsCompleted)
            {
               // NOTE: Между проверкой и получением элемента может возникнуть гонка
               int item = blockingCollection.Take();
               Console.WriteLine("Item: {0}", item);
            }
         });

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}