/**
 * Дочерняя присоединенная задача
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _10_AttachedChildTask
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
               Console.WriteLine("Child 1 running");
               Thread.Sleep(1000);
               Console.WriteLine("Child 1 finished");
               throw new Exception();
            }, TaskCreationOptions.AttachedToParent);

            // Создаем присоединенную задачу продолжения
            childTask.ContinueWith(task =>
            {
               Console.WriteLine("Continuation running");
               Thread.Sleep(1000);
               Console.WriteLine("Continuation finished");
            }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("Starting child task...");
            childTask.Start();
         });

         parentTask.Start();

         try
         {
            Console.WriteLine("Waiting for parent task");
            parentTask.Wait();
            Console.WriteLine("Parent task finished");
         }
         catch (AggregateException ex)
         {
            Console.WriteLine("Exception: {0}", ex.InnerException.GetType());
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}