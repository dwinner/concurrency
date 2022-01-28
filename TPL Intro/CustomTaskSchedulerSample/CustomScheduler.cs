using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _20_CustomTaskSchedulerSample
{
   public class CustomScheduler : TaskScheduler, IDisposable
   {
      private readonly BlockingCollection<Task> _taskQueue;
      private readonly Thread[] _threads;

      public CustomScheduler(int concurrency)
      {
         _taskQueue = new BlockingCollection<Task>();
         _threads = new Thread[concurrency];
         // Создаем и запускаем потоки
         for (int i = 0; i < _threads.Length; i++)
         {
            (_threads[i] = new Thread(() =>
            {
               foreach (Task task in _taskQueue.GetConsumingEnumerable())
               {
                  TryExecuteTask(task);
               }
            })).Start();
         }
      }

      public CustomScheduler()
         : this(Environment.ProcessorCount)
      {
      }

      public override int MaximumConcurrencyLevel
      {
         get { return _threads.Length; }
      }

      public void Dispose()
      {
         _taskQueue.CompleteAdding();
         foreach (Thread thread in _threads)
         {
            thread.Join();
         }
      }

      protected override void QueueTask(Task task)
      {
         if (task.CreationOptions.HasFlag(TaskCreationOptions.LongRunning))
         {
            new Thread(() => TryExecuteTask(task)).Start();
         }
         else
         {
            _taskQueue.Add(task);
         }
      }

      protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
      {
         return _threads.Contains(Thread.CurrentThread) && TryExecuteTask(task);
      }

      protected override IEnumerable<Task> GetScheduledTasks()
      {
         return _taskQueue.ToArray();
      }
   }
}