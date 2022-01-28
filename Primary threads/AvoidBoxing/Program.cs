/**
 * Остерегайтесь упаковки
 */

using System;
using System.Threading;

namespace _12_AvoidBoxing
{
   class Program
   {
      private static int _counter;
      private const int TheLock = 0; // NOTE: НИКОГДА НЕ ПОСТУПАЙТЕ ТАК

      private static void ThreadFunc()
      {
         for (int i = 0; i < 50; i++)
         {
            Monitor.Enter(TheLock); // Note: Неявная упаковка
            try
            {
               Console.WriteLine(++_counter);
            }
            finally
            {
               Monitor.Exit(TheLock);  // Note: Неявная упаковка, объект другой: SynchronizationLockException
            }
         }
      }

      static void Main()
      {
         var thread1 = new Thread(ThreadFunc);
         var thread2 = new Thread(ThreadFunc);
         thread1.Start();
         thread2.Start();

         Console.ReadKey();
      }
   }
}
