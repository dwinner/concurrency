using System;
using System.Threading.Tasks.Dataflow;

namespace InternalExceptionHandling
{
   internal static class InternalExceptionHandlingEx
   {
      private static void Main()
      {
         var divideBlock = new ActionBlock<(int, int)>(
            tuple =>
            {
               try
               {
                  Console.WriteLine(tuple.Item1 / tuple.Item2);
               }
               catch (DivideByZeroException)
               {
                  Console.WriteLine("Dude, can't divide by zero");
               }
            }
         );

         divideBlock.Post((10, 5));
         divideBlock.Post((20, 4));
         divideBlock.Post((10, 0));
         divideBlock.Post((10, 2));

         Console.ReadLine();
      }
   }
}