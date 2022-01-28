using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsynchronousQueues
{
   internal static class ThrottlingSample
   {
      private readonly static AsyncProducerConsumerQueue<int> AsyncQueue = new AsyncProducerConsumerQueue<int>(1);

      internal static async void Go()
      {
         await Task.WhenAll(Produce(), Consume());
      }

      private static async Task Consume()
      {
         while (true)
         {
            var dequeueResult = await AsyncQueue.TryDequeueAsync();
            if (!dequeueResult.Success)
            {
               break;
            }

            Console.WriteLine(dequeueResult.Item);
         }
      }

      private static async Task Produce()
      {
         // Эта вставка в очередь автоматически помечает ее завершенной для добавления
         await AsyncQueue.EnqueueAsync(7);

         // Эта вставка в очередь "ждет" в асинхронном режиме, пока элемент 7 не будет удален кодом producer'а,
         // перед тем как добавить 13
         await AsyncQueue.EnqueueAsync(13);

         // Помечаем очередь как завершенную
         AsyncQueue.CompleteAdding();
      }
   }
}