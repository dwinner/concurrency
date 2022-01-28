/**
 * Неизменяемые множества
 */

using System;
using System.Collections.Immutable;

namespace ImmutableSets
{
   internal static class Program
   {
      private static void Main()
      {
         IImmutableSet<int> hashSet = ImmutableHashSet<int>.Empty;
         hashSet = hashSet.Add(13);
         hashSet = hashSet.Add(7);

         foreach (var item in hashSet)
         {
            Console.WriteLine(item);
         }

         hashSet = hashSet.Remove(7);

         foreach (var item in hashSet)
         {
            Console.WriteLine(item);
         }

         IImmutableSet<int> sortedSet = ImmutableSortedSet<int>.Empty;
         sortedSet = sortedSet.Add(13);
         sortedSet = sortedSet.Add(7);

         foreach (var item in sortedSet)
         {
            Console.WriteLine(item);
         }
      }
   }
}