/**
 * Unhandled exeption example
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace UnhandledException
{
   internal static class UnhandledErrorEx
   {
      private static void Main()
      {
         var divideBlock = new ActionBlock<Tuple<int, int>>(input => Console.WriteLine(input.Item1 / input.Item2));

         divideBlock.Post(Tuple.Create(10, 5));
         divideBlock.Post(Tuple.Create(20, 4));
         divideBlock.Post(Tuple.Create(10, 0));
         divideBlock.Post(Tuple.Create(10, 2));
      }
   }
}