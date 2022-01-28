/**
 * Создание потоков выполнения
 */

using System;
using System.Threading;

namespace CustomThread
{
   class Program
   {
      static void Main()
      {
         Thread thread = new Thread(stateObject =>
         {
            int end = (int)stateObject;
            // Имитация работы
            for (int i = 0; i < end; ++i)
            {
               if (i % 100000000 == 0)
               {
                  Console.WriteLine("Thread ID: {0}, i: {1}",
                     Thread.CurrentThread.ManagedThreadId, i);
               }
            }
         }) { IsBackground = true };
         thread.Start(Int32.MaxValue); // Аргумент процедуры потока

         while (!Console.KeyAvailable)
         {
            Console.WriteLine("Thread ID: {0}, waiting for key press",
               Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
         }

         var timer = new Timer(stateObject =>
         {
            int val = (int)stateObject;
         }, 13, 1000, 10000);
      }
   }
}
