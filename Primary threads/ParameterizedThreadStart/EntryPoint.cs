/**
 * Передача информации в точку входа потока
 */

using System;
using System.Collections;
using System.Threading;

namespace _03_ParameterizedThreadStart
{
   class EntryPoint
   {
      static void Main()
      {
         var queue1 = new Queue();
         var queue2 = new Queue();

         // ... операции наполнения очередей данными
         // Обработка каждой очереди в отдельном потоке.
         var procThread1 = new Thread(ThreadFunc);
         procThread1.Start(queue1);
         var procThread2 = new Thread(ThreadFunc);
         procThread2.Start(queue2);

         // ... между тем выполнять какую-то другую работу.
         // Ожидать окончания работы.
         procThread1.Join();
         procThread2.Join();

         Console.ReadKey();
      }

      private static void ThreadFunc(object obj)
      {
         // Выполнить приведение входящего объекта
         var theQueue = obj as Queue;
         if (theQueue != null)
         {
            Console.WriteLine("Enqueue/Dequeue elements in {0}", Thread.CurrentThread.ManagedThreadId); // Опустошить очередь
         }
      }
   }
}
