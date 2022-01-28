using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PipelineViaTplDataFlow
{
   internal static class PipelineActivities
   {
      private static readonly char[] WordSeparators = { ' ', ';', '(', ')', '{', '}', '.', ',' };

      public static IEnumerable<string> GetFileNames(string path)
      {
         return Directory.EnumerateFiles(path, "*.cs");
      }

      public static IEnumerable<string> LoadLines(IEnumerable<string> fileNames)
      {
         foreach (var fileName in fileNames)
         {
            using (FileStream stream = File.OpenRead(fileName))
            {
               var reader = new StreamReader(stream);
               string line;
               while ((line = reader.ReadLine()) != null)
               {
                  yield return line;
               }
            }
         }
      }

      public static IEnumerable<string> GetWords(IEnumerable<string> lines)
      {
         return
            lines.Select(line => line.Split(WordSeparators))
               .SelectMany(words => words.Where(word => !string.IsNullOrEmpty(word)));
      }
   }
}