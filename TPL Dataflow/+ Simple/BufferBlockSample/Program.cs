/**
 * Блоки источника и приемника
 */

using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BufferBlockSample
{
   internal static class Program
   {
      private static readonly BufferBlock<string> BufferBlock = new BufferBlock<string>();

      private static void Main()
      {
         var producerTask = Task.Run(() => Producer());
         var consumerTask = Task.Run(() => Consumer());
         Task.WaitAll(producerTask, consumerTask);
      }

      private static void Producer()
      {
         var exit = false;
         while (!exit)
         {
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || string.Compare(input, "exit", StringComparison.OrdinalIgnoreCase) == 0)
            {
               exit = true;
            }
            else
            {
               BufferBlock.Post(input);
            }
         }
      }

      private static async void Consumer()
      {
         while (true)
         {
            var data = await BufferBlock.ReceiveAsync();
            Console.WriteLine("user input: {0}", data);
         }
      }
   }
}