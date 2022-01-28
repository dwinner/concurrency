using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PiplelineSample
{
   public static class PipelineStages
   {
      public static Task ReadFileNamesAsync(string path, BlockingCollection<string> output)
      {
         return Task.Run(() =>
         {
            #region Чтение в параллельном режиме

            //Parallel.ForEach(Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories), fileName =>
            //{
            //   output.Add(fileName);
            //   ConsoleHelper.WriteLine(string.Format("Stage 1: Added {0}", fileName));
            //});

            #endregion

            foreach (var filename in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
            {
               output.Add(filename);
            }
            output.CompleteAdding();
         });
      }

      public static async Task LoadContentAsync(BlockingCollection<string> input, BlockingCollection<string> output)
      {
         #region Загрузка содержимого в параллельном режиме

         //Parallel.ForEach(input.GetConsumingEnumerable(), filename =>
         //{
         //   using (FileStream stream = File.OpenRead(filename))
         //   {
         //      var reader = new StreamReader(stream);
         //      string line;
         //      while ((line = reader.ReadLine()) != null)
         //      {
         //         output.Add(line);
         //         ConsoleHelper.WriteLine(string.Format("Stage 2: added {0}", line));
         //      }
         //   }
         //});

         #endregion

         foreach (var filename in input.GetConsumingEnumerable())
         {
            using (var stream = File.OpenRead(filename))
            {
               var reader = new StreamReader(stream);
               string line;
               while ((line = await reader.ReadLineAsync()) != null)
               {
                  output.Add(line);
                  ConsoleHelper.WriteLine(string.Format("Stage 2: added {0}", line));
               }
            }
         }
      }

      public static Task ProcessContentAsync(BlockingCollection<string> input, ConcurrentDictionary<string, int> output)
      {
         return Task.Run(() =>
         {
            #region Обработка содержимого в параллельном режиме

            //Parallel.ForEach(input.GetConsumingEnumerable(), line =>
            //{
            //   string[] words = line.Split(' ', ';', '\t', '{', '}', '(', ')', ':', ',', '"');
            //   Parallel.ForEach(words.Where(w => !string.IsNullOrEmpty(w)), word =>
            //   {
            //      output.AddOrIncrementValue(word);
            //      ConsoleHelper.WriteLine(string.Format("stage 3: added {0}", word));
            //   });
            //});

            #endregion

            foreach (var line in input.GetConsumingEnumerable())
            {
               var words = line.Split(' ', ';', '\t', '{', '}', '(', ')', ':', ',', '"');
               foreach (var word in words.Where(w => !string.IsNullOrEmpty(w)))
               {
                  output.AddOrIncrementValue(word);
                  ConsoleHelper.WriteLine(string.Format("stage 3: added {0}", word));
               }
            }
         });
      }

      public static Task TransferContentAsync(ConcurrentDictionary<string, int> input, BlockingCollection<Info> output)
      {
         return Task.Run(() =>
         {
            #region Передача содержимого в параллельном режиме

            //Parallel.ForEach(input.Keys, aWord =>
            //{
            //   int value;
            //   if (input.TryGetValue(aWord, out value))
            //   {
            //      var info = new Info { Word = aWord, Count = value };
            //      output.Add(info);
            //      ConsoleHelper.WriteLine(string.Format("Stage 4: added {0}", info));
            //   }
            //});

            #endregion

            foreach (var word in input.Keys)
            {
               int value;
               if (input.TryGetValue(word, out value))
               {
                  var info = new Info {Word = word, Count = value};
                  output.Add(info);
                  ConsoleHelper.WriteLine(string.Format("Stage 4: added {0}", info));
               }
            }
            output.CompleteAdding();
         });
      }

      public static Task AddColorAsync(BlockingCollection<Info> input, BlockingCollection<Info> output)
      {
         return Task.Run(() =>
         {
            #region Изменение содержимого в параллельном режиме

            //Parallel.ForEach(input.GetConsumingEnumerable(), item =>
            //{
            //   int itemCount = item.Count;
            //   item.Color = itemCount > 40 ? "Red" : itemCount > 20 ? "Yellow" : "Green";
            //   output.Add(item);
            //   ConsoleHelper.WriteLine(string.Format("Stage 5: added color {1} to {0}", item, item.Color));
            //});

            #endregion

            foreach (var item in input.GetConsumingEnumerable())
            {
               var itemCount = item.Count;
               item.Color = itemCount > 40 ? "Red" : itemCount > 20 ? "Yellow" : "Green";
               output.Add(item);
               ConsoleHelper.WriteLine(string.Format("Stage 5: added color {1} to {0}", item, item.Color));
            }
            output.CompleteAdding();
         });
      }

      public static Task ShowContentAsync(BlockingCollection<Info> input)
      {
         return Task.Run(() =>
         {
            foreach (var item in input.GetConsumingEnumerable())
            {
               ConsoleHelper.WriteLine(string.Format("Stage 6: {0}", item), item.Color);
            }
         });
      }
   }
}