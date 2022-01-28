/**
 * Конвейерный алгоритм на основе потоков данных
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace PipelineViaTplDataFlow
{
   class Program
   {
      private const string RootPath = @"../..";

      static void Main()
      {
         var target = PipelineInstaller.SetupPipeline();
         target.Post(RootPath);
         Console.ReadLine();
      }
   }
}
