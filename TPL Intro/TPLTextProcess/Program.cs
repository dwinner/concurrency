/**
 * Параллельное выполнение заданий
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TPLTextProcess
{
   class Program
   {
      private static readonly ConcurrentDictionary<char, UInt64> charCount =
         new ConcurrentDictionary<char, UInt64>(Environment.ProcessorCount, 255);

      private static readonly ConcurrentDictionary<string, UInt64> wordCount =
         new ConcurrentDictionary<string, UInt64>(Environment.ProcessorCount, 100000,
            StringComparer.InvariantCultureIgnoreCase);

      static void Main()
      {
         string[] inputFiles = 
            {
                "decline1.txt", "decline2.txt", "decline3.txt",
                "decline4.txt", "decline5.txt", "decline6.txt"
            };

         Stopwatch watch = new Stopwatch();

         // Note: Итеративные вычисления

         Console.WriteLine("Iterative");
         watch.Start();

         foreach (string content in inputFiles.Select(File.ReadAllText))
         {
            CountCharacters(content);
            CountWords(content);
         }

         watch.Stop();
         Console.WriteLine("Elapsed: {0}", watch.Elapsed);
         Console.WriteLine("Unique chars: {0}", charCount.Keys.Count);
         Console.WriteLine("Unique words: {0}", wordCount.Keys.Count);

         watch.Reset();
         Console.WriteLine();

         // Note: Частично параллельные вычисления

         ClearContainers();
         Console.WriteLine("Partial parallel");
         watch.Start();
         foreach (string content in inputFiles.Select(File.ReadAllText))
         {
            string localContent = content;
            Parallel.Invoke(
               () => CountCharacters(localContent),
               () => CountWords(localContent)
            );
         }
         watch.Stop();
         Console.WriteLine("Elapsed: {0}", watch.Elapsed);
         Console.WriteLine("Unique chars: {0}", charCount.Keys.Count);
         Console.WriteLine("Unique words: {0}", wordCount.Keys.Count);

         watch.Reset();
         Console.WriteLine();

         // Note: Параллельные вычисления

         ClearContainers();
         Console.WriteLine("Parallel");
         watch.Start();
         Parallel.ForEach(inputFiles.Select(File.ReadAllText), text => Parallel.Invoke(
            () => CountCharacters(text),
            () => CountWords(text)
         ));
         watch.Stop();
         Console.WriteLine("Elapsed: {0}", watch.Elapsed);
         Console.WriteLine("Unique chars: {0}", charCount.Keys.Count);
         Console.WriteLine("Unique words: {0}", wordCount.Keys.Count);

         Console.ReadKey();
      }

      private static void ClearContainers()
      {
         charCount.Clear();
         wordCount.Clear();
      }

      static private void CountCharacters(string content)
      {
         foreach (char aChar in content)
         {
            if (charCount.ContainsKey(aChar))
               ++charCount[aChar];
            else
               charCount[aChar] = 1;
         }
      }

      static private void CountWords(string content)
      {
         List<char> splitChars = new List<char>();
         //for simplicity, consider everything that isn't a letter to be a separator
         //since input is ASCII, only need up to 255
         for (int c = 0; c <= 255; ++c)
         {
            if (!char.IsLetter((char)c))
            {
               splitChars.Add((char)c);
            }
         }
         string[] words = content.Split(splitChars.ToArray<char>(), StringSplitOptions.RemoveEmptyEntries);
         foreach (string word in words)
         {
            if (wordCount.ContainsKey(word))
               ++wordCount[word];
            else
               wordCount[word] = 1;
         }
      }
   }
}
