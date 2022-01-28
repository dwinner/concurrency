using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace PipelineViaTplDataFlow
{
   internal static class PipelineInstaller
   {
      public static ITargetBlock<string> SetupPipeline()
      {
         var fileNamesForPathBlock =
            new TransformBlock<string, IEnumerable<string>>(
               path => PipelineActivities.GetFileNames(path));
         var linesBlock =
            new TransformBlock<IEnumerable<string>, IEnumerable<string>>(
               fileNames => PipelineActivities.LoadLines(fileNames));
         var wordsBlock =
            new TransformBlock<IEnumerable<string>, IEnumerable<string>>(
               wordLines => PipelineActivities.GetWords(wordLines));

         var displayBlock = new ActionBlock<IEnumerable<string>>(outputs =>
         {
            foreach (var output in outputs)
            {
               Console.WriteLine(output);
            }
         });

         fileNamesForPathBlock.LinkTo(linesBlock);
         linesBlock.LinkTo(wordsBlock);
         wordsBlock.LinkTo(displayBlock);

         return fileNamesForPathBlock;
      }
   }
}