using HybridThreading.Library;
using System;
using System.Threading;

namespace SimpleHybridLock
{
   public sealed class HybridLock : IDisposable, IBlocking
   {
      // int используется примитивными конструкциями пользовательского режима Interlocked-методами
      private int _waiters;

      // AutoResetEvent - примитивная конструкция режима ядра
      private readonly AutoResetEvent _waiterLock = new AutoResetEvent(false);

      public void Enter()
      {
         // Указываем, что этот поток нуждается в блокировании
         if (Interlocked.Increment(ref _waiters) == 1)
         {
            return;  // Блокирование доступно, конкуренции нет, возвращаем управление
         }

         // Ожидаем другой поток. Блокируем его, так как наличествует конкуренция
         _waiterLock.WaitOne();  // Значительное снижение производительности
         // Когда WaitOne возвращает управление, этот поток блокируется
      }

      public void Leave()
      {
         // Этот поток отпирается
         if (Interlocked.Decrement(ref _waiters) == 0)
         {
            return;  // Другие потоки не заблокированы, возвращаем управление
         }

         // Другие потоки заблокированы, пробуждаем один из них
         _waiterLock.Set();   // Значительное снижение производительности
      }

      public void Dispose()
      {
         _waiterLock.Dispose();
      }
   }
}