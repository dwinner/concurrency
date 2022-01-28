/**
 * Простой блок передачи данных
 */

using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace BasicTransformBlock
{
   internal static class Program
   {
      private static void Main()
      {
         Example1();
         Example2();

         Console.Write("Press any key to finish");
         Console.ReadKey();
      }

      private static async void Example2() // NOTE: Асинхронный прием данных
      {
         Func<int, int> fn = i =>
         {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return i * i;
         };

         var transformBlock = new TransformBlock<int, int>(fn);

         for (var i = 0; i < 10; i++)
         {
            transformBlock.Post(i);
         }

         for (var i = 0; i < 10; i++)
         {
            var result = await transformBlock.ReceiveAsync();
            Console.WriteLine(result);
         }

         Console.WriteLine("Done");
      }

      private static void Example1() // NOTE: Синхронный прием данных
      {
         Func<int, int> fn = n =>
         {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return n * n;
         };

         var transformBlock = new TransformBlock<int, int>(fn);

         for (var i = 0; i < 10; i++)
         {
            transformBlock.Post(i);
         }

         for (var i = 0; i < 10; i++)
         {
            var result = transformBlock.Receive();
            Console.WriteLine(result);
         }

         Console.WriteLine("Done");
      }
   }
}