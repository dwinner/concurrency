using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PipelineSample
{
   public class Pipeline<TInput, TOutput>
   {
      private BlockingCollection<ValueCallBackWrapper> _valueQueue;
      private readonly Func<TInput, TOutput> _pipelineFunction;

      public Pipeline(Func<TInput, TOutput> pipelineFunction)
      {
         _pipelineFunction = pipelineFunction;
      }

      public Pipeline<TInput, TNewOutput> AddFunction<TNewOutput>(Func<TOutput, TNewOutput> newFunction)
      {
         return new Pipeline<TInput, TNewOutput>(inputValue => newFunction(_pipelineFunction(inputValue)));
      }

      public void AddValue(TInput value, Action<TInput, TOutput> callback)
      {
         _valueQueue.Add(new ValueCallBackWrapper { Value = value, Callback = callback });
      }

      public void StartProcessing()
      {
         _valueQueue = new BlockingCollection<ValueCallBackWrapper>();
         Task.Factory.StartNew(() =>
         {
            Parallel.ForEach(_valueQueue.GetConsumingEnumerable(),
               wrapper => wrapper.Callback(
                  wrapper.Value, _pipelineFunction(wrapper.Value)));
         });
      }

      public void StopProcessing()
      {
         _valueQueue.CompleteAdding();
      }

      private class ValueCallBackWrapper
      {
         internal TInput Value;
         internal Action<TInput, TOutput> Callback;
      }
   }
}