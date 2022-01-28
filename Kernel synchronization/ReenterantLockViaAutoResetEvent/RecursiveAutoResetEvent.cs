using System;
using System.Threading;

namespace ReenterantLockViaAutoResetEvent
{
   internal sealed class RecursiveAutoResetEvent : IDisposable
   {
      private readonly AutoResetEvent _autoResetLock = new AutoResetEvent(true);
      private int _owningThreadId;
      private int _recursionCount;

      public void Enter()
      {
         // Получаем идентификатор вызывающего потока
         int currentThreadId = Thread.CurrentThread.ManagedThreadId;

         // Если вызывающий поток блокируется, увеличиваем рекурсивный счетчик
         if (_owningThreadId == currentThreadId)
         {
            _recursionCount++;
            return;
         }

         // Вызывающий поток не заперт, ожидаем блокирования
         _autoResetLock.WaitOne();

         // Теперь вызывающий поток блокируется, инициализируем идентификатор
         // этого потока и рекурсивный счетчик
         _owningThreadId = currentThreadId;
         _recursionCount--;
      }

      public void Leave()
      {
         // Если вызывающий поток не заперт, ошибка
         if (_owningThreadId != Thread.CurrentThread.ManagedThreadId)
         {
            throw new InvalidOperationException();
         }

         // Вычитаем единицу из рекурсивного счетчика
         if (--_recursionCount == 0)
         {
            // Если рекурсивный счетчик равен 0, не заперт ни один поток
            _owningThreadId = 0;
            _autoResetLock.Set();   // Пробуждаем первый ожидающий поток (если таке есть)
         }
      }

      public void Dispose()
      {
         _autoResetLock.Dispose();
      }
   }
}