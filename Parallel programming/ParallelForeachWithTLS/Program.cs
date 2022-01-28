/**
 * TLS в циклах Foreach
 */

using System;
using System.Threading.Tasks;

namespace _11_ParallelForeachWithTLS
{
   internal static class Program
   {
      private static void Main()
      {
         var matchedWords = 0;
         var lockObject = new object();

         string[] dataItems =
         {
            "an", "apple", "a", "day", "keeps", "the", "doctor", "away"
         };

         Parallel.ForEach(
            dataItems,
            () => 0,
            (item, loopState, tlsValue) =>
            {
               if (item.Contains("a"))
               {
                  tlsValue++;
               }

               return tlsValue;
            },
            tlsValue =>
            {
               lock (lockObject)
               {
                  matchedWords += tlsValue;
               }
            });

         Console.WriteLine("Matched: {0}", matchedWords);
      }
   }
}