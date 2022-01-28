/**
 * Задачи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSamples
{
   class Program
   {
      private readonly static object TaskMethodLock = new object();

      static void Main()
      {
         TaskUsingThreadPool();
         RunSynchronousTask();
         LongRunningTask();
         TaskWithResult();
         ContinuationTask();
         ParentAndChild();
         ParentAndChild2();

         Console.ReadKey();
      }

      private static void TaskMethod(object state)
      {
         lock (TaskMethodLock)
         {
            Console.WriteLine(state);
            Console.WriteLine("Task id: {0}, Thread: {1}",
               Task.CurrentId == null ? "no task" : Task.CurrentId.ToString(),
               Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Is pooled thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Is background thread: {0}", Thread.CurrentThread.IsBackground);
            Console.WriteLine();
         }
      }

      #region Задачи, использующие пул потоков

      private static void TaskUsingThreadPool()
      {
         var taskFactory = new TaskFactory();
         /*Task t1 = */
         taskFactory.StartNew(TaskMethod, "Using a task factory");

         /*Task t2 = */
         Task.Factory.StartNew(TaskMethod, "factory via task");

         var t3 = new Task(TaskMethod, "using a task constructor and start");
         t3.Start();

         /*Task t4 = */
         Task.Run(() => TaskMethod("using the run method"));
      }

      #endregion

      #region Синхронные задачи

      private static void RunSynchronousTask()
      {
         TaskMethod("just the main thread");
         var t1 = new Task(TaskMethod, "run sync");
         t1.RunSynchronously();
      }

      #endregion

      #region Задачи, использующие отдельный поток

      private static void LongRunningTask()
      {
         var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
         t1.Start();
      }

      #endregion

      #region Получение результатов из задач

      private static Tuple<int, int> TaskWithResult(object division)
      {
         var div = (Tuple<int, int>)division;
         int result = div.Item1 / div.Item2;
         int reminder = div.Item1 % div.Item2;
         Console.WriteLine("task creates a result...");

         return Tuple.Create(result, reminder);
      }

      private static void TaskWithResult()
      {
         var t1 = new Task<Tuple<int, int>>(TaskWithResult, Tuple.Create(8, 3));
         t1.Start();
         Console.WriteLine(t1.Result); // NOTE: блокирует вызывающий поток
         t1.Wait();
         Console.WriteLine("result from task: {0} {1}", t1.Result.Item1, t1.Result.Item2);
      }

      #endregion

      #region Задачи продолжения

      private static void DoOnFirst()
      {
         Console.WriteLine("doing some task {0}", Task.CurrentId);
         Thread.Sleep(3000);
      }

      private static void DoOnSecond(Task t)
      {
         Console.WriteLine("task {0} finished", t.Id);
         Console.WriteLine("this task id: {0}", Task.CurrentId);
         Console.WriteLine("do some cleanup");
         Thread.Sleep(3000);
      }

      private static void DoOnError(Task t)
      {
         Console.WriteLine("task {0} had an error!", t.Id);
         Console.WriteLine("my id {0}", Task.CurrentId);
         Console.WriteLine("do some cleanup");
      }

      private static void ContinuationTask()
      {
         var t1 = new Task(DoOnFirst);

         Task t2 = t1.ContinueWith(DoOnSecond);
         /*Task t3 = */
         t1.ContinueWith(DoOnSecond);
         /*Task t5 = */
         t1.ContinueWith(DoOnError, TaskContinuationOptions.OnlyOnFaulted);

         /*Task t4 = */
         t2.ContinueWith(DoOnSecond);

         t1.Start();
         Thread.Sleep(5000);
      }

      #endregion

      #region Иерархии задач

      private static void ParentAndChild()
      {
         var parent = new Task(ParentTask);
         parent.Start();
         Thread.Sleep(2000);
         Console.WriteLine(parent.Status);
         Thread.Sleep(4000);
         Console.WriteLine(parent.Status);
      }

      private static void ParentTask()
      {
         Console.WriteLine("task id {0}", Task.CurrentId);
         var child = new Task(ChildTask);
         child.Start();
         Thread.Sleep(1000);
         Console.WriteLine("parent started child");
      }

      private static void ChildTask()
      {
         Console.WriteLine("child");
         Thread.Sleep(5000);
         Console.WriteLine("child finished");
      }

      private static void ParentAndChild2()
      {
         var factory = new TaskFactory();
         Task newTask = factory.StartNew(() =>
         {
            Console.WriteLine("parent task {0}", Task.CurrentId);

            factory.StartNew(() =>
            {
               Console.WriteLine("child task {0}", Task.CurrentId);
               Thread.Sleep(2000);
               Console.WriteLine("finished child");
            }, TaskCreationOptions.AttachedToParent);
         });

         newTask.Wait();
      }

      #endregion
   }
}
