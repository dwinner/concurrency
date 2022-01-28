/**
 * Альтернативное создание задачи
 */

using System;
using System.Threading.Tasks;

namespace CreatingTasks
{
   internal class Program
   {
      private static void Main()
      {
         var theTask = new Task(() => Console.WriteLine("Hello"));
         theTask.Start();
         Console.ReadLine();
      }
   }
}