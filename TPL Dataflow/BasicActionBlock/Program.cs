/**
 * Простой блок действия
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace BasicActionBlock
{
   internal static class Program
   {
      private static void Main()
      {
         var actionBlock = new ActionBlock<int>(i => Console.WriteLine(i));
         for (var i = 0; i < 10; i++)
         {
            actionBlock.Post(i);
         }

         Console.WriteLine("Done");

         Console.ReadKey();
      }
   }
}