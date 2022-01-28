/**
 * Пулы потоков
 */

using System;
using System.Threading;

namespace ThreadPoolSamples
{
   internal static class Program
   {
      private static void Main()
      {
         int nWorkerThreads;
         int nCompletionPortThreads;
         ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionPortThreads);
         Console.WriteLine("Max worker threads: {0}, I/O completion threads: {1}", nWorkerThreads,
            nCompletionPortThreads);

         for (var i = 0; i < 5; i++)
         {
            ThreadPool.QueueUserWorkItem(Job);
         }

         Thread.Sleep(3000);
      }

      private static void Job(object state)
      {
         for (var i = 0; i < 3; i++)
         {
            Console.WriteLine("loop {0}, running inside pooled thread {1}", i, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(50);
         }
      }
   }
}