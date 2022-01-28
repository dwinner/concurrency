/**
 * Один из примеров реализации конвейерных алгоритмов
 * на базе коллекций, безопасных в отношении потоков
 */

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace PiplelineSample
{
   internal static class Program
   {
      private const string FileNamesPath = @"d:\Programming and Development\";
      private static TextWriter _oldWriter;

      static void Main()
      {
         _oldWriter = Console.Out;
         Console.SetOut(new StreamWriter("OutResults.txt"));
         StartPipeline();
         Console.ReadLine();
      }

      private static async void StartPipeline() // Управление конвейером
      {
         var fileNames = new BlockingCollection<string>();     // 1) Имена файлов
         var lines = new BlockingCollection<string>();         // 2) Загрузка содержимого
         var words = new ConcurrentDictionary<string, int>();  // 3) Обработка содержимого
         var items = new BlockingCollection<Info>();           // 4) Передача содержимого
         var coloredItems = new BlockingCollection<Info>();    // 5) Добавление информации о цвете

         Task readFileNamesTask = PipelineStages.ReadFileNamesAsync(FileNamesPath, fileNames);
         ConsoleHelper.WriteLine("Started stage 1");
         Task loadContentTask = PipelineStages.LoadContentAsync(fileNames, lines);
         ConsoleHelper.WriteLine("Started stage 2");
         Task processContentTask = PipelineStages.ProcessContentAsync(lines, words);
         await Task.WhenAll(readFileNamesTask, loadContentTask, processContentTask);
         ConsoleHelper.WriteLine("Stages 1, 2, 3 completed");

         Console.SetOut(_oldWriter);

         Task transferContentTask = PipelineStages.TransferContentAsync(words, items);
         Task addColorTask = PipelineStages.AddColorAsync(items, coloredItems);
         Task showContentTask = PipelineStages.ShowContentAsync(coloredItems);

         await Task.WhenAll(transferContentTask, addColorTask, showContentTask);

         ConsoleHelper.WriteLine("All stages finished");
      }
   }
}
