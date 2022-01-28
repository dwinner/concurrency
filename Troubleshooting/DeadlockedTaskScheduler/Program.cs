/**
 * Deadlock в планировщике задач
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockedTaskScheduler
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем планировщик задач
         var scheduler = new DeadlockedTaskScheduler(5);

         var tokenSource = new CancellationTokenSource();

         var tasks = new Task[6];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = Task.Factory.StartNew(o =>
            {
               var index = (int)o;
               if (index < tasks.Length - 1)
               {
                  Console.WriteLine("Task {0} waiting for {1}", index, index + 1);
                  tasks[index + 1].Wait(tokenSource.Token);
               }

               Console.WriteLine("Task {0} complete", index);
            }, i, tokenSource.Token, TaskCreationOptions.None, scheduler);
         }

         Task.WaitAll(tasks);
         Console.WriteLine("All tasks complete");

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class DeadlockedTaskScheduler : TaskScheduler, IDisposable
   {
      private readonly BlockingCollection<Task> _taskQueue;
      private readonly Thread[] _threads;

      public DeadlockedTaskScheduler(int concurrency)
      {
         _taskQueue = new BlockingCollection<Task>();
         _threads = new Thread[concurrency];
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
         return false; // BUG: делаем недоступным выполнение вне очереди
      }

      protected override IEnumerable<Task> GetScheduledTasks()
      {
         return _taskQueue.ToArray();
      }
   }
}