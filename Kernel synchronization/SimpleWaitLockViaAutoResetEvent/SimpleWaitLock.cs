using System;
using System.Threading;

namespace SimpleWaitLockViaAutoResetEvent
{
   public sealed class SimpleWaitLock : IDisposable
   {
      private readonly AutoResetEvent _resourceFree = new AutoResetEvent(true); // Изначально свободно

      public void Enter()
      {
         // Блокируем в ядре ресурсы, которые должны быть свободны,
         // и возвращаем управление
         _resourceFree.WaitOne();
      }

      public void Leave()
      {
         _resourceFree.Set(); // помечаем ресурс как свободный
      }

      public void Dispose()
      {
         _resourceFree.Dispose();
      }
   }
}