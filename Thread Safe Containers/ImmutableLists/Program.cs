/**
 * Неизменяемый список
 */

using System;
using System.Collections.Immutable;

namespace ImmutableLists
{
   internal static class Program
   {
      private static void Main()
      {
         var list = ImmutableList<int>.Empty;
         list = list.Insert(0, 13);
         list = list.Insert(0, 7);
         list.ForEach(Console.WriteLine);
         list = list.RemoveAt(1);

         // Лучший способ итерации по неизменяемому списку
         foreach (var item in list)
         {
            Console.WriteLine(item);
         }

         // Этот способ тоже работает, но гораздо медленнее
         for (var i = 0; i != list.Count; i++)
         {
            Console.WriteLine(list[i]);
         }
      }
   }
}