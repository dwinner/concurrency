/**
 * Неизменяемые словари
 */

using System;
using System.Collections.Immutable;

namespace ImmutableDictionaries
{
   internal static class Program
   {
      private static void Main()
      {
         var dictionary = ImmutableDictionary<int, string>.Empty;
         dictionary = dictionary.Add(10, "Ten");
         dictionary = dictionary.Add(21, "Twenty-One");
         dictionary = dictionary.SetItem(10, "Diez");

         foreach (var item in dictionary)
         {
            Console.WriteLine("{0} {1}", item.Key, item.Value);
         }

         //var ten = dictionary[10];
         //dictionary = dictionary.Remove(21);

         var sortedDictionary = ImmutableSortedDictionary<int, string>.Empty;
         sortedDictionary = sortedDictionary.Add(10, "Ten");
         sortedDictionary = sortedDictionary.Add(21, "Twenty-One");
         sortedDictionary = sortedDictionary.SetItem(10, "Diez");

         foreach (var item in sortedDictionary)
         {
            Console.WriteLine("{0} {1}", item.Key, item.Value);
         }

         // ...etc
      }
   }
}