/**
 * Опции потоков данных
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Dataflow;

// ReSharper disable FunctionNeverReturns

namespace BlockExecutionOptions
{
   internal static class Program
   {
      private static void Main()
      {
         Example1();
         Benchmark1();
         Benchmark2();

         Console.ReadKey();
      }

      private static void Benchmark2()
      {
         var sw = new Stopwatch();
         const int iters = 6000000;
         var are = new AutoResetEvent(false);

         var ab = new ActionBlock<int>(i => { if (i == iters) are.Set(); },
            new ExecutionDataflowBlockOptions { SingleProducerConstrained = true });
         while (true)
         {
            sw.Restart();
            for (var i = 1; i <= iters; i++) ab.Post(i);
            are.WaitOne();
            sw.Stop();
            Console.WriteLine("Messages per sec (single): {0:N0}", (iters / sw.Elapsed.TotalSeconds));
         }
      }

      private static void Benchmark1()
      {
         var sw = new Stopwatch();
         const int iters = 6000000;
         var are = new AutoResetEvent(false);

         var ab = new ActionBlock<int>(i =>
         {
            if (i == iters)
               are.Set();
         });

         while (true)
         {
            sw.Restart();
            for (var i = 1; i <= iters; i++) ab.Post(i);
            are.WaitOne();
            sw.Stop();
            Console.WriteLine("Messages per sec: {0:N0}", (iters / sw.Elapsed.TotalSeconds));
         }
      }

      private static void Example1()
      {
         var generator = new Random();
         Action<int> fn = i =>
         {
            Thread.Sleep(generator.Next(1000));
            Console.WriteLine(i);
         };

         var dataflowBlockOptions = new ExecutionDataflowBlockOptions
         {
            MaxDegreeOfParallelism = 2
         };

         var actionBlock = new ActionBlock<int>(fn, dataflowBlockOptions);

         for (var i = 0; i < 10; i++)
         {
            actionBlock.Post(i);
         }

         Console.WriteLine("Done");
      }
   }
}