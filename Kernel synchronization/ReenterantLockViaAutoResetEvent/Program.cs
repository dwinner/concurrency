/**
 * Рекурсивное блокирование при помощи объекта AutoResetEvent
 */

using System;
using System.Threading;

namespace ReenterantLockViaAutoResetEvent
{
   static class Program
   {
      static void Main()
      {
         var recursiveAutoResetEvent = new RecursiveAutoResetEvent();
         var threads = new Thread[5];
         for (int i = 0; i < threads.Length; i++)
         {
            threads[i] = new Thread(() =>
            {
               recursiveAutoResetEvent.Enter();
               Shared.ShareField++;
               recursiveAutoResetEvent.Leave();
            });
            threads[i].Start();
         }

         Array.ForEach(threads, thread => thread.Join());
      }
   }

   class Shared
   {
      internal static int ShareField = 0;
   }
}
