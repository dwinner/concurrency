/**
 * Неизменяемый стек
 */

using System;
using System.Collections.Immutable;

namespace ImmutableStackSample
{
   internal static class Program
   {
      private static void Main()
      {
         var stack = ImmutableStack<int>.Empty;
         stack = stack.Push(13);
         stack = stack.Push(7);

         foreach (var item in stack)
         {
            Console.WriteLine(item);
         }

         int popItem;
         stack = stack.Pop(out popItem);

         foreach (var item in stack)
         {
            Console.WriteLine(item);
         }
      }
   }
}