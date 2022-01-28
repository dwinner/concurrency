/**
 * Опции продолжения задач
 */

using System;
using System.Threading.Tasks;

namespace _04_ContinueOptions
{
   internal static class Program
   {
      private static void Main()
      {
         var firstGetTask = new Task(() =>
         {
            Console.WriteLine("Message from first generation task");
            throw new Exception();
         });

         /*Task secondGenTask1 = */
         firstGetTask.ContinueWith(task =>
            {
               if (task.Exception != null)
                  Console.WriteLine("Antecedent task faulted with type: {0}", task.Exception.GetType());
            }, TaskContinuationOptions.OnlyOnFaulted);

         /*Task secondGenTask2 = */
         firstGetTask.ContinueWith(task => Console.WriteLine("Antecedent task NOT faulted"),
            TaskContinuationOptions.NotOnFaulted);

         firstGetTask.Start();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}