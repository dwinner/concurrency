/**
 * Реализация пула через семафор
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace _15_Semaphore
{
   class EntryPoint
   {
      static void WorkFunction()
      {
         Console.WriteLine("Метод WorkFunction() вызван на потоке {0}",
            Thread.CurrentThread.ManagedThreadId);
      }

      static void Main()
      {
         var pool = new CrudeThreadPool();
         for (int i = 0; i < 10; i++)
         {
            pool.SubmitWorkItem(WorkFunction);
         }
         Thread.Sleep(2000);  // Задержка для эмуляции выполнения данным потоком другой работы
         pool.Shutdown();

         Console.ReadKey();
      }
   }

   public class CrudeThreadPool
   {
      private const int MaxWorkThreads = 4;
      private const int WaitTimeout = 2000;

      private readonly Semaphore _semaphore;
      private readonly Queue<WorkDelegate> _workQueue;
      private volatile bool _stop;

      public delegate void WorkDelegate();

      public CrudeThreadPool()
      {
         _stop = false;
         _semaphore = new Semaphore(0, int.MaxValue);
         _workQueue = new Queue<WorkDelegate>();
         var threads = new Thread[MaxWorkThreads];
         for (int i = 0; i < MaxWorkThreads; i++)
         {
            threads[i] = new Thread(ThreadFunc);
            threads[i].Start();
         }
      }

      private void ThreadFunc()
      {
         do
         {
            if (!_stop && _semaphore.WaitOne(WaitTimeout))
            {
               // Обработать элемент из начала очереди.
               WorkDelegate workItem;
               lock (_workQueue)
               {
                  workItem = _workQueue.Dequeue();
               }
               workItem();
            }
         } while (!_stop);
      }

      public void SubmitWorkItem(WorkDelegate item)
      {
         lock (_workQueue)
         {
            _workQueue.Enqueue(item);
         }
         _semaphore.Release();
      }

      public void Shutdown()
      {
         _stop = true;
      }
   }
}
