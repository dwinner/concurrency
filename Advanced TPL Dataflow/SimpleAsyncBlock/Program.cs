using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SimpleAsyncBlock
{
   internal static class Program
   {
      private static void Main()
      {
         async Task AsyncBody(int arg)
         {
            Console.WriteLine("Running");
            for (var i = 0; i < arg; i++)
            {
               Console.WriteLine("{0}:{1}", Thread.CurrentThread.ManagedThreadId, i);
               await Task.Delay(TimeSpan.FromSeconds(1));
            }
         }

         var consumeBlock = new ActionBlock<int>(AsyncBody);

         while (true)
         {
            consumeBlock.Post(5);
            Console.ReadLine();
         }
         // ReSharper disable FunctionNeverReturns
      }
      // ReSharper restore FunctionNeverReturns
   }
}