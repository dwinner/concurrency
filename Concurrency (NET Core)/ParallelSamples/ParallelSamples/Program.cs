using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSamples
{
   internal static class Program
   {
      private static readonly Command[] Commands =
      {
         new Command("-p", nameof(ParallelFor), ParallelFor),
         new Command("-pfa", nameof(ParallelForWithAsync), ParallelForWithAsync),
         new Command("-spfe", nameof(StopParallelForEarly), StopParallelForEarly),
         new Command("-pfi", nameof(ParallelForWithInit), ParallelForWithInit),
         new Command("-pfe", nameof(ParallelForEach), ParallelForEach),
         new Command("-pi", nameof(ParallelInvoke), ParallelInvoke)
      };

      private static void Main(string[] args)
      {
         if (args.Length is 0 or > 1 || !Commands.Select(c => c.Option).Contains(args[0]))
         {
            ShowUsage();
            return;
         }

         Commands.Single(c => c.Option == args[0]).Action();

         Console.ReadLine();
      }

      private static void ShowUsage()
      {
         Console.WriteLine("Usage: ParallelSamples [options]");
         Console.WriteLine();
         foreach (var command in Commands)
         {
            Console.WriteLine($"{command.Option} {command.Text}");
         }
      }

      public static void ParallelInvoke()
      {
         Parallel.Invoke(Foo, Bar, Foo, Bar, Foo, Bar);
      }

      private static void Foo() =>
         Console.WriteLine("foo");

      private static void Bar() =>
         Console.WriteLine("bar");

      public static void ParallelForEach()
      {
         string[] data =
         {
            "zero",
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "ten",
            "eleven",
            "twelve"
         };
         var result = Parallel.ForEach(data, s => { Console.WriteLine(s); });
         Console.WriteLine(result.IsCompleted);
      }

      public static void ParallelFor()
      {
         var result = Parallel.For(0, 10, i =>
         {
            Log($"S {i}");
            Task.Delay(10).Wait();
            Log($"E {i}");
         });
         Console.WriteLine($"Is completed: {result.IsCompleted}");
      }

      public static void ParallelForWithAsync()
      {
         var result = Parallel.For(0, 10, async i =>
         {
            Log($"S {i}");
            await Task.Delay(10);
            Log($"E {i}");
         });
         Console.WriteLine($"Is completed: {result.IsCompleted}");
      }

      public static void StopParallelForEarly()
      {
         var result = Parallel.For(10, 40, (int i, ParallelLoopState pls) =>
         {
            Log($"S {i}");
            if (i > 12)
            {
               pls.Break();
               Log($"break now {i}");
            }

            Task.Delay(10).Wait();
            Log($"E {i}");
         });

         Console.WriteLine($"Is completed: {result.IsCompleted}");
         Console.WriteLine($"lowest break iteration: {result.LowestBreakIteration}");
      }

      public static void ParallelForWithInit()
      {
         Parallel.For(0, 100, () =>
            {
               // invoked once for each thread
               Log("init thread");
               return $"t{Thread.CurrentThread.ManagedThreadId}";
            },
            (i, pls, str1) =>
            {
               // invoked for each member
               Log($"body i: {i} str1: {str1}");
               Task.Delay(10).Wait();
               return $"i {i}";
            },
            (str1) =>
            {
               // final action on each thread
               Log($"finally {str1}");
            });
      }

      private static void Log(string prefix) =>
         Console.WriteLine($"{prefix} task: {Task.CurrentId}, thread: {Thread.CurrentThread.ManagedThreadId}");
   }
}