/**
 * Политика обработки исключений
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationPolicy
{
   internal static class Program
   {
      private static void Main()
      {
         TaskScheduler.UnobservedTaskException += (sender, args) =>
         {
            args.SetObserved(); // Помечаем исключение как обработанное
            args.Exception.Handle(exception =>
            {
               Console.WriteLine("Exception type: {0}", exception.GetType());
               return true;
            });
         };

         var task1 = new Task(() => { throw new NullReferenceException(); });
         var task2 = new Task(() => { throw new ArgumentOutOfRangeException(); });

         task1.Start();
         task2.Start();

         while (!task1.IsCompleted || !task2.IsCompleted)
         {
            Thread.Sleep(500);
         }

         Console.WriteLine("Press enter to finish and finalize tasks");
         Console.ReadLine();
      }
   }
}