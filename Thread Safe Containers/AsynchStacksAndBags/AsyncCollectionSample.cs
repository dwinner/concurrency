using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsynchStacksAndBags
{
   internal sealed class AsyncCollectionSample
   {
      private readonly AsyncCollection<int> _asyncCollection;
      private readonly int _throttleThreshold;

      internal AsyncCollectionSample(IProducerConsumerCollection<int> producerConsumer, int throttleThreshold = 0)
      {
         _asyncCollection = new AsyncCollection<int>(producerConsumer);
         _throttleThreshold = throttleThreshold;
      }

      internal async void Go()
      {
         await Task.WhenAll(Producer(), Consumer());
      }

      private async Task Consumer()
      {
         if (_throttleThreshold > 0)
         {
            while (true)
            {
               var takeResult = await _asyncCollection.TryTakeAsync();
               if (!takeResult.Success)
               {
                  break;
               }

               Console.WriteLine(takeResult.Item);
            }
         }
         else
         {
            while (await _asyncCollection.OutputAvailableAsync())
            {
               Console.WriteLine(await _asyncCollection.TakeAsync());
            }
         }
      }

      private async Task Producer()
      {
         await _asyncCollection.AddAsync(7);
         await _asyncCollection.AddAsync(13);

         _asyncCollection.CompleteAdding();
      }
   }
}