/**
 * Отложенное выполнение задач
 */

using System;
using System.Threading.Tasks;

namespace LazyTaskExecution
{
   internal static class Program
   {
      private static void Main()
      {
         Func<string> taskBody = () =>
         {
            Console.WriteLine("Task body working...");
            return "Task result";
         };

         var lazyData = new Lazy<Task<string>>(() => Task.Factory.StartNew(taskBody));

         Console.WriteLine("Calling lazy variable");
         Console.WriteLine("Result from task: {0}", lazyData.Value.Result);

         var lazyData2 = new Lazy<Task<string>>(() =>
            Task<string>.Factory.StartNew(() =>
            {
               Console.WriteLine("Task body working...");
               return "Task result";
            }));

         Console.WriteLine("Calling second lazy variable");
         Console.WriteLine("Result from task: {0}", lazyData2.Value.Result);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}