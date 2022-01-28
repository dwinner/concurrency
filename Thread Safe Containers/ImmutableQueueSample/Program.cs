/**
 * Неизменяемая очередь
 */

using System;
using System.Collections.Immutable;

namespace ImmutableQueueSample
{
   internal static class Program
   {
      private static void Main()
      {
         var queue = ImmutableQueue<int>.Empty;
         queue = queue.Enqueue(13);
         queue = queue.Enqueue(7);

         foreach (var item in queue)
         {
            Console.WriteLine(item);
         }

         int nextItem;
         queue = queue.Dequeue(out nextItem);

         foreach (var item in queue)
         {
            Console.WriteLine(item);
         }
      }
   }
}