/**
 * Обычный перехват исключений
 */

using System;
using System.Threading.Tasks;

namespace BasicHandling
{
   internal static class Program
   {
      private static void Main()
      {
         var task1 = new Task(() => { throw new ArgumentOutOfRangeException { Source = "task1" }; });

         var task2 = new Task(() => { throw new NullReferenceException(); });

         var task3 = new Task(() => Console.WriteLine("Hello from Task 3"));

         task1.Start();
         task2.Start();
         task3.Start();

         try
         {
            Task.WaitAll(task1, task2, task3);
         }
         catch (AggregateException aggregateEx)
         {
            foreach (Exception innerEx in aggregateEx.InnerExceptions)
            {
               Console.WriteLine("Exception type {0} from {1}", innerEx.GetType(), innerEx.Source);
            }
         }

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}