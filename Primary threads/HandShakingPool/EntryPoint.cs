/**
 * Реализация пула рабочих потоков через квитирование.
 */

using System;
using System.Collections;
using System.Threading;

namespace _14_PoolViaHandShaking
{
   internal static class EntryPoint
   {
      private static void WorkFunction()
      {
         Console.WriteLine("Метод WorkFunction() вызван на потоке {0}",
            Thread.CurrentThread.ManagedThreadId);
      }

      private static void Main()
      {
         var pool = new CrudeThreadPool();
         for (var i = 0; i < 10; i++)
         {
            pool.SubmitWorkItem(WorkFunction);
         }
         // Задержка для эмуляции выполнения данным потоком другой работы
         Thread.Sleep(1000);
         pool.Shutdown();

         Console.ReadKey();
      }
   }

   public class CrudeThreadPool
   {
      public delegate void WorkDelegate();

      private const int MaxWorkThreads = 4;
      private const int WaitTimeout = 2000;
      private readonly object _workLock;

      private readonly Queue _workQueue;
      private volatile bool _stop;

      public CrudeThreadPool()
      {
         _stop = false;
         _workLock = new object();
         _workQueue = new Queue();
         var threads = new Thread[MaxWorkThreads];
         for (var i = 0; i < MaxWorkThreads; i++)
         {
            threads[i] = new Thread(ThreadFunc);
            threads[i].Start();
         }
      }

      private void ThreadFunc()
      {
         lock (_workLock)
         {
            do
            {
               if (_stop)
                  continue;
               WorkDelegate workItem;
               if (!Monitor.Wait(_workLock, WaitTimeout))
                  continue;
               // Обработать элемент из начала очереди.
               lock (_workQueue.SyncRoot)
               {
                  workItem = (WorkDelegate) _workQueue.Dequeue();
               }
               workItem();
            } while (!_stop);
         }
      }

      public void SubmitWorkItem(WorkDelegate item)
      {
         lock (_workLock)
         {
            lock (_workQueue.SyncRoot)
            {
               _workQueue.Enqueue(item);
            }
            Monitor.Pulse(_workLock);
         }
      }

      public void Shutdown()
      {
         _stop = true;
      }
   }
}