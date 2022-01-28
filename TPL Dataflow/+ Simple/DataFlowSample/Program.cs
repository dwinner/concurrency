using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace DataFlowSample
{
   class Program
   {
      static void Main()
      {
         var target = SetupPipeline();
         target.Post("../..");
         Console.ReadLine();
      }

      static ITargetBlock<string> SetupPipeline()
      {

         var fileNames = new TransformBlock<string, IEnumerable<string>>(path =>
         {
            try
            {
               return GetFileNames(path);
            }
            catch (OperationCanceledException)
            {
               return Enumerable.Empty<string>();
            }
         });

         var loadLines = new TransformBlock<IEnumerable<string>, IEnumerable<string>>(input =>
             {
                try
                {
                   return LoadLines(input);
                }
                catch (OperationCanceledException)
                {
                   return Enumerable.Empty<string>();
                }
             });

         var action = new ActionBlock<IEnumerable<string>>(s => Console.WriteLine("action {0}", s));

         //var split = new TransformBlock<IEnumerable<string>, IDictionary<string, int>>(input =>
         //  {
         //    return Dictionary<string, int>
         //  });

         //      fileNames.LinkTo(loadLines, new Predicate<IEnumerable<string>((IEnumerable<string> fileNames2) => fileNames2.Count() > 0));

         fileNames.LinkTo(loadLines, fn => fn.Any());
         loadLines.LinkTo(action);

         return fileNames;
      }

      static IEnumerable<string> LoadLines(IEnumerable<string> fileNames)
      {
         foreach (var fileName in fileNames)
         {
            using (FileStream stream = File.OpenRead(fileName))
            {
               var reader = new StreamReader(stream);
               string line;
               while ((line = reader.ReadLine()) != null)
               {
                  Console.WriteLine("LoadLines {0}", line);
                  yield return line;
               }
            }
         }
      }

      static IEnumerable<string> GetFileNames(string path)
      {
         foreach (var fileName in Directory.EnumerateFiles(path, "*.cs"))
         {
            Console.WriteLine("GetFileNames {0}", fileName);
            yield return fileName;
         }
      }
   }
}
