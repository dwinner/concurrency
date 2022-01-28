/**
 * Компоненты с поддержкой асинхронных вычислений
 */

using System;
using System.Threading;

namespace Wrox.ProCSharp.Threading
{
   internal static class Program
   {
      private static void Main()
      {
         Console.WriteLine("Main thread: {0}", Thread.CurrentThread.ManagedThreadId);

         var asyncComponent = new AsyncComponent();
         asyncComponent.LongTaskCompleted += Comp_LongTaskCompleted;

         asyncComponent.LongTaskAsync("input", 33);
         Console.ReadLine();
      }

      private static void Comp_LongTaskCompleted(object sender, LongTaskCompletedEventArgs e)
      {
         Console.WriteLine("completed result: {0}, thread: {1}", e.Output, Thread.CurrentThread.ManagedThreadId);
      }
   }
}