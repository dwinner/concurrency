using HybridThreading.Library;
using System;
using System.Threading;

namespace ComplexHybridLock
{
   public sealed class AnotherHybridLock : IBlocking, IDisposable
   {
      private int _waiters;  // int используется примитивом в пользовательском режиме (методы Interlocked)
      private readonly AutoResetEvent _waiterLock = new AutoResetEvent(false);   // Примитивная конструкция режима ядра
      private const int SpinCount = 4000; // Это поле контролирует зацикливание с целью поднять производительность
      private int _owningThreadId; // Какой поток блокмруется
      private int _recursion;   // Сколько раз блокируется

      public void Enter()
      {
         // Если вызывающий поток уже заперт, увеличим рекурсивный счетчик на единицу и вернем управление
         int threadId = Thread.CurrentThread.ManagedThreadId;
         if (threadId == _owningThreadId)
         {
            _recursion++;
            return;
         }

         // Вызываюющий поток не заперт, пытаемся получить право на блокирование
         var spinWait = new SpinWait();
         for (int spinCount = 0; spinCount < SpinCount; spinCount++)
         {
            // Если блокирование возможно, этот поток блокируется
            // Задаем некоторое состояние и возвращаем управление
            if (Interlocked.CompareExchange(ref _waiters, 1, 0) == 0)
            {
               goto GotLock;
            }

            // Черная магия: даем остальным потокам шанс запуститься в надежде на отпирание
            spinWait.SpinOnce();
         }

         // Время зацикливания истекает, а отпирания еще не случилось, пытаемся еще раз
         if (Interlocked.Increment(ref _waiters) > 1)
         {
            // Остальные потоки заблокированы и этот тоже должен быть заблокирован
            _waiterLock.WaitOne();  // Ожидаем возможности блокирования; производительность падает
            // Проснувшись, этот поток получает право на блокирование
            // Задаем некоторое состояние и возвращаем управление
         }

      GotLock:
         // Когда поток блокируется, записываем его идентификатор
         // и указываем, что он получил право на блокирование впервые
         _owningThreadId = threadId;
         _recursion = 1;
      }

      public void Leave()
      {
         // Если вызывающий поток не заперт, ошибка
         int threadId = Thread.CurrentThread.ManagedThreadId;
         if (threadId != _owningThreadId)
         {
            throw new SynchronizationLockException("Lock not owned by calling thread");
         }

         // Уменьшаем на единицу рекурсивный счетчик. Если поток все еще заперт, просто возвращаем управление
         if (--_recursion > 0)
         {
            return;
         }

         _owningThreadId = 0; // Запертых потоков больше нет

         // Если нет других заблокированных потоков, возвращаем управление
         if (Interlocked.Decrement(ref _waiters) == 0)
         {
            return;
         }

         // Остальные потоки заблокированы, пробуждаем один из них
         _waiterLock.Set();   // Значительное падение производительности
      }

      public void Dispose()
      {
         _waiterLock.Dispose();
      }
   }
}