/**
 * Запуск потоков
 */

using System;
using System.Threading;

namespace _01_StartTheThread
{
   class EntryPoint
   {
      static void Main()
      {
         // Создание нового потока
         var newThread = new Thread(ThreadFunc);
         Console.WriteLine("Main thread: {0}", Thread.CurrentThread.ManagedThreadId);
         Console.WriteLine("Start a new thread...");

         // Запуск нового потока
         newThread.Start();

         // Ожидание завершения работы нового потока.
         newThread.Join();
         Console.WriteLine("New thread is finished");

         Console.ReadKey();
      }

      private static void ThreadFunc()
      {
         Console.WriteLine("Hello from thread {0}!",
             Thread.CurrentThread.GetHashCode());
      }
   }
}
