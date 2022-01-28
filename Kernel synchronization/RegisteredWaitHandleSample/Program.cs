/**
 * Вызов метода в потоке пула при получении доступа
 * к конструкции в режиме ядра
 */

using System;
using System.Threading;

namespace RegisteredWaitHandleSample
{
   internal static class Program
   {
      public static void Main()
      {
         // Создание объекта AutoResetEvent (изначально несвободного)
         var autoResetEvent = new AutoResetEvent(false);

         // Заставляем пул ждать AutoResetEvent
         RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
            autoResetEvent,   // Ждем этого объекта WaitHandle
            EventOperation,   // При получении доступа вызываем метод EventOperation
            null,             // Передаем null в EventOperation
            5000,             // Ждем освобождения 5 сек.
            false);           // Вызываем EventOperation при каждом освобождении

         // Начало цикла
         var operation = (char)0;
         while (operation != 'Q')
         {
            Console.WriteLine("S=Signal, Q=Quit?");
            operation = char.ToUpper(Console.ReadKey(true).KeyChar);
            if (operation == 'S')
            {
               autoResetEvent.Set();   // Пользователь хочет задать событие
            }
         }

         // Заставляем пул прекратить ожидание события
         registeredWaitHandle.Unregister(null);
      }

      // Этот метод вызывается при каждом освобождении события
      // или через 5 сек. после обратного вызова/завершения времени ожидания
      private static void EventOperation(object state, bool timedout)
      {
         Console.WriteLine(timedout ? "Timeout" : "Event became true");
      }
   }
}
