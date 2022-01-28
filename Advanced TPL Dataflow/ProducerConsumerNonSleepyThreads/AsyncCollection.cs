using System.Collections.Concurrent;
using System.Threading.Tasks;
// ReSharper disable InconsistentlySynchronizedField

namespace ProducerConsumerNonSleepyThreads
{
   public class AsyncCollection<T>
   {
      private readonly IProducerConsumerCollection<T> _collection;
      private readonly ConcurrentQueue<TaskCompletionSource<T>> _consumers;

      public AsyncCollection(IProducerConsumerCollection<T> collection)
      {
         _collection = collection;
         _consumers = new ConcurrentQueue<TaskCompletionSource<T>>();
      }

      public Task<T> TakeAsync()
      {
         var taskCompletionSource = new TaskCompletionSource<T>();
         if (!TryGetItem(taskCompletionSource))
            lock (_collection)
            {
               if (!TryGetItem(taskCompletionSource))
                  _consumers.Enqueue(taskCompletionSource);
            }

         return taskCompletionSource.Task;
      }

      public void Add(T item)
      {
         if (!TryWaitingConsumer(item))
            lock (_collection)
            {
               if (!TryWaitingConsumer(item))
                  _collection.TryAdd(item);
            }
      }

      private bool TryWaitingConsumer(T item)
      {
         if (_consumers.TryDequeue(out var taskCompletion))
         {
            Task.Run(() => taskCompletion.SetResult(item));
            return true;
         }

         return false;
      }

      private bool TryGetItem(TaskCompletionSource<T> taskCompletionSource)
      {
         if (_collection.TryTake(out var item))
         {
            taskCompletionSource.SetResult(item);
            return true;
         }

         return false;
      }
   }
}