/**
 * Ситуация взаимной блокировки при зависимости задач
 */

using System;
using System.Threading.Tasks;

namespace DependencyDeadlock
{
   internal static class Program
   {
      private static void Main()
      {
         var tasks = new Task<int>[2];

         tasks[0] = Task.Factory.StartNew(() => tasks[1].Result + 100);
         tasks[1] = Task.Factory.StartNew(() => tasks[0].Result + 100);

         // ReSharper disable once CoVariantArrayConversion
         Task.WaitAll(tasks);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}