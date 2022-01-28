/**
 * Блок действия с задержками
 */

using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace ActionBlockDelay
{
   internal static class Program
   {
      private static void Main()
      {
         var actionBlock = new ActionBlock<int>(i =>
         {
            Thread.Sleep(1000);
            Console.WriteLine(i);
         });

         for (var i = 0; i < 10; i++)
         {
            actionBlock.Post(i);
         }

         Console.WriteLine("Done");

         Console.ReadKey();
      }
   }
}