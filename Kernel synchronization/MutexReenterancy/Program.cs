/**
 * Рекурсивное блокирование через Mutex
 */

using System;
using System.Threading;

namespace MutexReenterancy
{
   static class Program
   {
      static void Main()
      {
      }
   }

   class SomeClass : IDisposable
   {
      private readonly Mutex _lockMutex = new Mutex();

      public void Method1()
      {
         _lockMutex.WaitOne();
         // Делаем что-то...
         Method2();  // Метод Method2, рекурсивно получающий право на блокировку
         _lockMutex.ReleaseMutex();
      }

      private void Method2()
      {
         _lockMutex.WaitOne();
         // Делаем что-то...
         _lockMutex.ReleaseMutex();
      }

      public void Dispose()
      {
         _lockMutex.Dispose();
      }
   }
}
