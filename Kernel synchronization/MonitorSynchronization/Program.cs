/**
 * Синхронизация явным монитором объекта
 */

using System;
using System.Threading;

namespace MonitorSynchronization
{
   class Program
   {
      static void Main()
      {
         var lockObject = new object();
         bool lockTaken = false;
         Monitor.TryEnter(lockObject, TimeSpan.FromMilliseconds(500), ref lockTaken);
         if (lockTaken)
         {
            try
            {
               // Получение блокировки.
               // Синхронизированная область для lockObject
            }
            finally
            {
               Monitor.Exit(lockObject);
            }
         }
         else
         {
            // Блокировка не получения, делать что-то другое
            Console.WriteLine("Do something else...");
         }
      }
   }
}
