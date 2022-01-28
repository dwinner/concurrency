using System;
using System.Threading;

namespace SemaphoreWaitLock
{
   public sealed class SimpleWaitLock : IDisposable
   {
      private readonly Semaphore _availableResources;

      public SimpleWaitLock()
         : this(Environment.ProcessorCount)
      { }

      public SimpleWaitLock(int maximumConcurrentThreads)
      {
         _availableResources = new Semaphore(maximumConcurrentThreads, maximumConcurrentThreads);
      }

      public void Enter()
      {
         // Ожидаем в ядре доступа к ресурсу и возвращаем управление
         _availableResources.WaitOne();
      }

      public void Leave()
      {
         // Этому потоку доступ больше не нужен; его может получить другой поток
         _availableResources.Release();
      }

      public void Dispose()
      {
         _availableResources.Close();
      }
   }
}