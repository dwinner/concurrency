using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace CustomBlocks
{
   public class Tap<T> : IPropagatorBlock<T, T>
   {
      private readonly List<ITargetBlock<T>> _links = new List<ITargetBlock<T>>();
      private T _toPropergate;

      public bool IsOpen { get; set; }

      public DataflowMessageStatus OfferMessage(
         DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
      {
         if (!IsOpen)
            return DataflowMessageStatus.Declined;

         if (consumeToAccept)
         {
            source.ConsumeMessage(messageHeader, this, out var messageConsumed);
            if (!messageConsumed)
               return DataflowMessageStatus.Declined;
         }

         _toPropergate = messageValue;
         _links.ForEach(target => target.OfferMessage(messageHeader, messageValue, this, true));

         return DataflowMessageStatus.Accepted;
      }

      public void Complete()
      {
      }

      public void Fault(Exception exception)
      {
      }

      public Task Completion { get; }

      public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
      {
         _links.Add(target);
         return null;
      }

      public T ConsumeMessage(
         DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
      {
         messageConsumed = true;
         return _toPropergate;
      }

      public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target) => true;

      public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
      {
      }
   }
}