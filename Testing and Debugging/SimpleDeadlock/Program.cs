/**
 * Простейший пример взаимной блокировки
 */

using System;
using System.Threading.Tasks;

namespace SimpleDeadlock
{
   internal static class Program
   {
      private static void Main()
      {
         var tasks = new Task[2];

         tasks[0] = Task.Factory.StartNew(() => { tasks[1].Wait(); });
         tasks[1] = Task.Factory.StartNew(() => { tasks[0].Wait(); });

         Console.WriteLine("Waiting for tasks to complete");
         Task.WaitAll();
      }
   }
}