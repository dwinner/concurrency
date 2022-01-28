/**
 * Дочерние задания
 */

using System;
using System.Threading.Tasks;

namespace _09_ChildTasks
{
   class Program
   {
      static void Main()
      {
         var parenTask = new Task<int[]>(() =>
         {
            var results = new int[3];  // Создание массива для результатов

            // Это задание создает и запускает 3 дочерних задания
            new Task(() => results[0] = Sum(10000),
               TaskCreationOptions.AttachedToParent).Start();
            new Task(() => results[1] = Sum(20000),
               TaskCreationOptions.AttachedToParent).Start();
            new Task(() => results[2] = Sum(30000),
               TaskCreationOptions.AttachedToParent).Start();

            // Возвращается ссылка на массив (элементы могут быть не инициализированы)
            return results;
         });

         // Вывод результатов после завершения родительского и дочерних заданий
         /*var cwt = */
         parenTask.ContinueWith(
            parent => Array.ForEach(parent.Result, Console.WriteLine));

         // Запуск родительского задания, которое запускает дочерние
         parenTask.Start();

         Console.ReadKey();
      }

      private static int Sum(int n)
      {
         int sum = 0;
         for (; n > 0; n--) checked { sum += n; }
         return sum;
      }
   }
}
