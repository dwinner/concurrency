/**
 * Шаблон условной переменной
 */

using System.Threading;

namespace ConditionalVariablePattern
{
   static class Program
   {
      static void Main()
      {
      }
   }

   internal sealed class ConditionalVariablePattern
   {
      private readonly object _lock = new object();
      private /*volatile*/ bool _condition;

      public void FirstThread()
      {
         Monitor.Enter(_lock);   // Взаимоисключающая блокировка

         // "Атомарная" проверка сложного условия блокирования
         while (!_condition)
         {
            // Если условие не соблюдается, ждем, что его поменяет другой поток
            Monitor.Wait(_lock); // На время снимаем блокировку, чтобы другой поток мог ее получить
         }

         // Условие соблюдено, обрабатываем данные...

         Monitor.Exit(_lock); // Снятие блокировки
      }

      public void SecondThread()
      {
         Monitor.Enter(_lock);
         _condition = true;   // Обрабатываем данные и изменяем условие...
         Monitor.PulseAll(_lock);   // Будем всех ожидающих после отмены блокировки
         Monitor.Exit(_lock); // Отпирание
      }
   }
}
