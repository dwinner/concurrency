/**
 * Завершение заданий и получение результата
 */

using System;
using System.Threading.Tasks;

namespace _06_FinishingTaskAndResult
{
   class EntryPoint
   {
      static void Main()
      {
         // Создание задания Task (оно пока не выполняется)
         var task = new Task<int>(n => Sum((int)n), 1000/*000000*/);

         // Можно начать выполнение задания через некоторое время
         task.Start();

         // Можно ожидать завершения задания в явном виде
         task.Wait();   // NOTE: Существует перегруженная версия, принимающая timeout/CancellationToken

         // Получение результата (свойство Result вызывает метод Wait)
         Console.WriteLine("Сумма: {0}", task.Result);

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
