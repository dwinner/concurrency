/**
 * Передача данных новым потокам
 */

using System;
using System.Collections;

namespace _02_ThreadSendingData
{
   class EntryPoint
   {
      static void Main()
      {
         var queue1 = new Queue();
         var queue2 = new Queue();
         // ... операции наполнения очередей данными.
         // Обработка каждой очереди в отдельном потоке
         var processor1 = new QueueProcessor(queue1);
         processor1.BeginProcessData();
         var processor2 = new QueueProcessor(queue2);
         processor2.BeginProcessData();
         // ... между тем выполнять какую-то другую работу.
         // Ожидать окончания работы.
         processor1.EndProcessData();
         processor2.EndProcessData();

         Console.ReadKey();
      }
   }
}
