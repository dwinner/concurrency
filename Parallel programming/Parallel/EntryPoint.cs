/**
 * Методы класса Parallel
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _12_Parallel
{
   class EntryPoint
   {
      static void Main()
      {
         const string path = @"D:\C#.NET.Development";
         const string searchPattern = "*.cs";
         const SearchOption searchOption = SearchOption.AllDirectories;
         long directoryBytes = DirectoryBytes(path, searchPattern, searchOption);
         Console.WriteLine("All cs-files allocate: {0}", directoryBytes);
         Console.ReadKey();
      }

      private static long DirectoryBytes(string path, string searchPattern, SearchOption searchOption)
      {
         IEnumerable<string> files = Directory.EnumerateFiles(path, searchPattern, searchOption);
         long masterTotal = 0;
         /*ParallelLoopResult result = */
         Parallel.ForEach<string, long>(
            files,
            // localInit: вызывается в момент запуска задания
            // Устанавливает, что задача видит 0 байтов
            () => 0,
            // body: Вызывается один раз для каждого элемента
            // Получает размер файла и добавляет его к общему размеру
            (file, loopState, index, taskLocalTotal) =>
            {
               long fileLength = 0;
               try
               {
                  FileStream fileStream = null;
                  try
                  {
                     fileStream = File.OpenRead(file);
                     fileLength = fileStream.Length;
                  }
                  finally
                  {
                     if (fileStream != null)
                     {
                        fileStream.Dispose();
                     }
                  }
               }
               catch (IOException) { /* Игнорируем файлы, к которым нет доступа */ }
               catch (UnauthorizedAccessException) { /* Игнорируем файлы, к которым нет доступа */ }
               return taskLocalTotal + fileLength;
            },
            // localFinally: Вызывается один раз в конце задания
            // Добавляем размер из задания к общему размеру
            taskLocalTotal => Interlocked.Add(ref masterTotal, taskLocalTotal));
         return masterTotal;
      }
   }
}
