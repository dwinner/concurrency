/**
 * Возможности класса Parallel
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSamples
{
   class Program
   {
      static void Main()
      {
         SimpleParallelFor();
         BreakingParallelFor();
         AdvancedParallelFor();
         ParallelForeach();
         ParallelInvoke();
      }

      #region Простой сценарий использования Parallel.For

      private static void SimpleParallelFor()
      {
         ParallelLoopResult result = Parallel.For(0, 10, async i =>
         {
            Console.WriteLine("{0}, Task {1}, Thread: {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(10);
            Console.WriteLine("{0}, Task {1}, Thread: {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
         });
         Console.WriteLine("Is completed: {0}", result.IsCompleted);
      }

      #endregion

      #region Прерывание итерации в Parallel.For

      private static void BreakingParallelFor()
      {
         ParallelLoopResult result = Parallel.For(10, 40, (i, state) =>
         {
            Console.WriteLine("i: {0} task {1}", i, Task.CurrentId);
            Thread.Sleep(10);
            if (i > 15)
            {
               state.Break();
            }
         });
         Console.WriteLine("Is completed: {0}", result.IsCompleted);
         if (!result.IsCompleted)
         {
            Console.WriteLine("Lowest break iteration: {0}", result.LowestBreakIteration);
         }
      }

      #endregion

      #region Использование Parallel.For с локальной инициализацией, телом и завершением

      private static void AdvancedParallelFor()
      {
         Parallel.For(0, 20,
            () => // Локальная инициализация
            {
               Console.WriteLine("Init thread {0}, Task {1}",
                  Thread.CurrentThread.ManagedThreadId,
                  Task.CurrentId);
               return string.Format("t{0}", Thread.CurrentThread.ManagedThreadId);
            },
            (i, state, arg3) =>  // Тело
            {
               Console.WriteLine("Body i {0} arg3 {1} thread {2} task {3}",
                  i,
                  arg3,
                  Thread.CurrentThread.ManagedThreadId,
                  Task.CurrentId);
               Thread.Sleep(10);
               return string.Format("i {0}", i);
            },
            s => Console.WriteLine("finally {0}", s));   // Завершение
      }

      #endregion

      #region Параллельная итерация по коллекциям

      private static void ParallelForeach()
      {
         string[] data =
         {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve"
         };

         Parallel.ForEach(data, s => Console.WriteLine(s));
         Parallel.ForEach(data, (s, state, index) => Console.WriteLine("{0} {1}", s, index));
      }

      #endregion

      #region Параллельные вызовы методов

      private static void ParallelInvoke()
      {
         Parallel.Invoke(
            () => Console.WriteLine("Foo"),
            () => Console.WriteLine("Bar"));
      }

      #endregion

   }
}
