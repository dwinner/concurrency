/**
 * Автоматический запуск заданий по завершению предыдущего
 */

using System;
using System.Threading.Tasks;

namespace _08_ContinueWithTasks
{
   class Program
   {
      static void Main()
      {
         var task = new Task<int>(n => Sum((int)n), 10000);

         // Задание можно запустить позже
         task.Start();

         // Каждый метод ContinueWith возвращает Task, но это не имеет значения
         task.ContinueWith(aTask => Console.WriteLine("The Sum is: {0}", task.Result),
            TaskContinuationOptions.OnlyOnRanToCompletion);

         task.ContinueWith(aTask => Console.WriteLine("Sum threw: {0}", task.Exception),
            TaskContinuationOptions.OnlyOnFaulted);

         task.ContinueWith(aTask => Console.WriteLine("Sum was canceled"),
            TaskContinuationOptions.OnlyOnCanceled);

         Console.ReadKey();
      }

      private static int Sum(int n)
      {
         int sum = 0;
         for (; n > 0; n--)
         {
            checked
            {
               sum += n;   // При больших n появляется System.OverflowException
            }
         }
         return sum;
      }
   }
}
