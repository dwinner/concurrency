using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace IntervalBatcher
{
   internal static class Program
   {
      private static void Main()
      {
         var batcher = new BatchBlock<int>(int.MaxValue);
         var averager = new ActionBlock<int[]>(intValues => Console.WriteLine(intValues.Average()));

         batcher.LinkTo(averager);

         // ReSharper disable UnusedVariable
         var timer = new Timer(_ => batcher.TriggerBatch(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
         // ReSharper restore UnusedVariable

         var rnd = new Random();
         while (true)
            batcher.Post(rnd.Next(1, 100));
         // ReSharper disable FunctionNeverReturns
      }
      // ReSharper restore FunctionNeverReturns
   }
}