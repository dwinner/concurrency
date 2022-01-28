using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DecoupledConsoleSample
{
   public static class DecoupledConsole
   {
      private static readonly BlockingCollection<Action> _BlockingQueue;
      private static readonly Task _MessageWorkerTask;

      static DecoupledConsole()
      {
         _BlockingQueue = new BlockingCollection<Action>();
         _MessageWorkerTask = Task.Factory.StartNew(() =>
         {
            foreach (Action action in _BlockingQueue.GetConsumingEnumerable())
            {
               action();
            }
         }, TaskCreationOptions.LongRunning);
      }

      public static Task MessageWorker
      {
         get { return _MessageWorkerTask; }
      }

      public static void WriteLine(object value)
      {
         _BlockingQueue.Add(() => Console.WriteLine(value));
      }

      public static void WriteLine(string format, params object[] values)
      {
         _BlockingQueue.Add(() => Console.WriteLine(format, values));
      }
   }
}