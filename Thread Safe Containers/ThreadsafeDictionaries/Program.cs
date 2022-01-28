/**
 * Потокобезопасные словари
 */

using System.Collections.Concurrent;

namespace ThreadsafeDictionaries
{
   internal static class Program
   {
      private static void Main()
      {
         var dictionary = new ConcurrentDictionary<int, string>();
         var value = dictionary.AddOrUpdate(0,
            key => "Zero", // Вызывается, если ключ еще не существует в словаре
            (key, oldValue) => "Zero"); // Вызывется, если ключ уже существует в словаре
      }
   }
}