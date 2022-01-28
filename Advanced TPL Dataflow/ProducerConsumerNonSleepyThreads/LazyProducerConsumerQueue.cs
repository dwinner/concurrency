using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

// ReSharper disable FunctionNeverReturns

namespace ProducerConsumerNonSleepyThreads
{
   public class LazyProducerConsumerQueue<T>
   {
      private readonly Action<T> _consumer;
      private readonly AsyncCollection<T> _queue = new AsyncCollection<T>(new ConcurrentQueue<T>());

      public LazyProducerConsumerQueue(int numberOfConsumers, Action<T> consumer)
      {
         _consumer = consumer;
         for (var nConsumer = 0; nConsumer < numberOfConsumers; nConsumer++)
         {
#pragma warning disable 4014
            ConsumerBody();
#pragma warning restore 4014
         }
      }

      public void Enqueue(T item) => _queue.Add(item);

      private async Task ConsumerBody()
      {
         while (true)
         {
            var result = await _queue.TakeAsync().ConfigureAwait(false);
            _consumer(result);
         }
      }
   }
}