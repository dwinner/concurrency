/**
 * Многопоточное сжатие файлов с использованиев TPL DataFlow Actor'ов
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks.Dataflow;

namespace ConcurrentCompressing
{
   internal static class Program
   {
      private const int BufferSize = 0x4000;

      private static void Main(string[] args)
      {
         var inoutFiles = ParseArgs(args);

         var stopwatch = Stopwatch.StartNew();

         using (var inputStream = File.OpenRead(inoutFiles.Item1))
         using (var outputStream = File.Create(inoutFiles.Item2))
         {
            Compress(inputStream, outputStream);
         }

         stopwatch.Stop();

         Console.WriteLine();
         Console.WriteLine("Time elapsed: {0}s", stopwatch.Elapsed.TotalSeconds);
         Console.ReadKey();
      }

      private static Tuple<string, string> ParseArgs(IReadOnlyList<string> args)
      {
         return args != null && args.Count == 2
            ? Tuple.Create(args[0], args[1])
            : Tuple.Create(string.Empty, string.Empty);
      }

      /// <summary>
      ///    Сжатие байтов
      /// </summary>
      /// <param name="bytes">Исходный массив байтов</param>
      /// <returns>Сжатый массив байтов</returns>
      private static byte[] Compress(byte[] bytes)
      {
         using (var memoryStream = new MemoryStream())
         using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
         using (var binaryWriter = new BinaryWriter(gZipStream))
         {
            binaryWriter.Write(bytes);
            return memoryStream.ToArray();
         }
      }

      private static void Compress(Stream inputStream, Stream outputStream)
      {
         var buffer = new BufferBlock<byte[]>(new DataflowBlockOptions { BoundedCapacity = 100 });
         var compressor = new TransformBlock<byte[], byte[]>(bytes => Compress(bytes),
            new ExecutionDataflowBlockOptions
            {
               MaxDegreeOfParallelism = Environment.ProcessorCount,
               BoundedCapacity = 100
            });
         var writer = new ActionBlock<byte[]>(bytes => outputStream.Write(bytes, 0, bytes.Length),
            new ExecutionDataflowBlockOptions
            {
               BoundedCapacity = 100,
               SingleProducerConstrained = true
            });

         buffer.LinkTo(compressor);
         buffer.Completion.ContinueWith(task => compressor.Complete());
         compressor.LinkTo(writer);
         compressor.Completion.ContinueWith(task => writer.Complete());

         var readBuffer = new byte[BufferSize];
         while (true)
         {
            var readCount = inputStream.Read(readBuffer, 0, BufferSize);
            if (readCount > 0)
            {
               var postData = new byte[readCount];
               Buffer.BlockCopy(readBuffer, 0, postData, 0, readCount);
               while (!buffer.Post(postData))
               {
               }
            }

            if (readCount != BufferSize)
            {
               buffer.Complete();
               break;
            }
         }

         writer.Completion.Wait();
      }
   }
}