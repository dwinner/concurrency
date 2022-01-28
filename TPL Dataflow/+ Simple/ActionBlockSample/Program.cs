/**
 * Блок действия
 */

using System;
using System.Threading.Tasks.Dataflow;

namespace ActionBlockSample
{
   class Program
   {
      static void Main()
      {
         var processInput = new ActionBlock<string>(s => Console.WriteLine("user input: {0}", s));
         bool exit = false;
         while (!exit)
         {
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
               continue;
            }

            if (string.Compare(input, "exit", StringComparison.OrdinalIgnoreCase) == 0)
            {
               exit = true;
            }
            else
            {
               processInput.Post(input);
            }
         }
      }
   }
}
