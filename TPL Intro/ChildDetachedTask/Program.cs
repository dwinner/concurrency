/**
 * Дочерняя отсоединенная задача
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _09_ChildDetachedTask
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем родительскую задачу
         var parentTask = new Task(() =>
         {
            // Создаем первую дочернюю задачу
            var childTask = new Task(() =>
            {
               Console.WriteLine("Child task running");
               Thread.Sleep(1000);
               Console.WriteLine("Child task finished");
               throw new Exception();
            });

            Console.WriteLine("Starting child task");
            childTask.Start();
         });

         // Запускаем родительскую задачу
         parentTask.Start();

         Console.WriteLine("Waiting for parent task");
         parentTask.Wait();
         Console.WriteLine("Parent task finished");
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}