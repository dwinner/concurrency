using System.Collections.Concurrent;

namespace PiplelineSample
{
   public static class ConcurrentDictionaryExtension
   {
      public static void AddOrIncrementValue(this ConcurrentDictionary<string, int> dictionary, string key)
      {
         var success = false;
         while (!success)
         {
            int value;
            if (dictionary.TryGetValue(key, out value))
            {
               if (dictionary.TryUpdate(key, value + 1, value))
               {
                  success = true;
               }
            }
            else
            {
               if (dictionary.TryAdd(key, 1))
               {
                  success = true;
               }
            }
         }
      }
   }
}