/**
 * Развязанная консоль
 */

using System;
using System.Threading.Tasks;

namespace DecoupledConsoleSample
{
   static class Program
   {
      static void Main()
      {
         for (int i = 0; i < 10; i++)
         {
            Task.Factory.StartNew(state =>
            {
               for (int j = 0; j < 10; j++)
               {
                  DecoupledConsole.WriteLine("Message from task {0}", Task.CurrentId);
               }
            }, i);
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
