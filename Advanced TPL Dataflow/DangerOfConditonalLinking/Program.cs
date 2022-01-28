using System;
using System.Threading.Tasks.Dataflow;

namespace DangerOfConditonalLinking
{
   internal static class Program
   {
      private static void Main()
      {
         var block = new BufferBlock<int>();
         var nullBlock = DataflowBlock.NullTarget<int>();

         var oddBlock = new ActionBlock<int>(i => Console.WriteLine(i));

         block.LinkTo(oddBlock, i => i % 2 == 1);
         block.LinkTo(nullBlock);

         var j = 0;
         while (true)
            block.Post(++j);
         // ReSharper disable FunctionNeverReturns
      }
      // ReSharper restore FunctionNeverReturns
   }
}