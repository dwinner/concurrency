using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TransformManySample
{
   internal static class Program
   {
      private static void Main()
      {
         var consumeBlock = new ActionBlock<int>(new Action<int>(Consumer));
         var transformManyBlock = new TransformManyBlock<int, int>(new Func<int, IEnumerable<int>>(ProduceMany));
         transformManyBlock.LinkTo(consumeBlock);
         transformManyBlock.Post(10);

         Console.ReadLine();
      }

      private static void Consumer(int obj) => Console.WriteLine("{0}:Consuming {1}", Task.CurrentId, obj);

      private static IEnumerable<int> ProduceMany(int i)
      {
         for (var j = 0; j < i; j++)
         {
            Console.WriteLine("{0}: Generating {1}", Task.CurrentId, j);
            yield return j;
            Thread.Sleep(TimeSpan.FromSeconds(1));
         }
      }
   }
}