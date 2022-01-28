using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueSample
{
   /// <summary>
   /// Синхронная очередь
   /// </summary>
   /// <typeparam name="T">Параметр типа</typeparam>
   public sealed class SynchronizedQueue<T>
   {
      private readonly object _privateLock = new object();
      private readonly Queue<T> _queue = new Queue<T>();

      public void Enqueue(T item)
      {
         Monitor.Enter(_privateLock);
         _queue.Enqueue(item);   // После постановки элемента в очередь пробуждаем один/все ожидающие потоки
         Monitor.PulseAll(_privateLock);
         Monitor.Exit(_privateLock);
      }

      public T Dequeue()
      {
         Monitor.Enter(_privateLock);
         while (_queue.Count == 0)  // Выполняем цикл, пока очередь не опустеет (условие)
         {
            Monitor.Wait(_queue);
         }
         T item = _queue.Dequeue(); // Удаляем элемент из очереди и возвращаем его на обработку
         Monitor.Exit(_privateLock);

         return item;
      }
   }
}