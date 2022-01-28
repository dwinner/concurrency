using System;
using System.Collections;
using System.Threading;

namespace _02_ThreadSendingData
{
   public class QueueProcessor
   {
      private Queue _queue;
      private readonly Thread _thread;

      public Thread Thread
      {
         get { return _thread; }
      }

      public QueueProcessor(Queue queue)
      {
         _queue = queue;
         _thread = new Thread(ThreadFunc);
      }

      public void BeginProcessData()
      {
         _thread.Start();
      }

      public void EndProcessData()
      {
         _thread.Join();
      }

      private void ThreadFunc(object obj)
      {
         Console.WriteLine("Dequeue elements in {0}", Thread.CurrentThread.ManagedThreadId);
      }
   }
}