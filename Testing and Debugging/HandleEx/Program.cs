/**
 * Перехват исключений
 */

using System;
using System.Threading.Tasks;

namespace HandleEx
{
   internal static class Program
   {
      private static void Main()
      {
         var tasks = new Task[2];
         for (var i = 0; i < tasks.Length; i++)
         {
            tasks[i] = Task.Factory.StartNew(() =>
            {
               for (var j = 0; j < 5000000; j++)
               {
                  if (j == 500)
                  {
                     throw new Exception("Value is 500");
                  }
                  Math.Pow(j, 2);
               }
            });
         }

         try
         {
            Task.WaitAll(tasks);
         }
         catch (AggregateException ex)
         {
            ex.Handle(innerEx =>
            {
               Console.WriteLine("Exception message is {0}", innerEx.Message);
               return true;
            });
         }
      }
   }
}