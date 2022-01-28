/**
 * Получение результатов от задачи
 */

using System;
using System.Threading.Tasks;

namespace GettingResult
{
   internal class Program
   {
      private static void Main()
      {
         // Создаем задачу
         var firstTask = new Task<int>(() =>
         {
            int sum = 0;
            for (int i = 0; i < 100; i++)
            {
               sum += i;
            }

            return sum;
         });

         // Запускаем задачу
         firstTask.Start();

         // Выводим результат
         Console.WriteLine("First result: {0}", firstTask.Result);

         // Создаем задачу, используя передачу объекта состояния
         var secondTask = new Task<int>(o =>
         {
            int sum = 0;
            var max = (int)o;
            for (int i = 0; i < max; i++)
            {
               sum += i;
            }

            return sum;
         }, 100);

         // Запускаем задачу
         secondTask.Start();

         // Выводим результат
         Console.WriteLine("Second result: {0}", secondTask.Result);

         // Возвращение результатов методом StartNew<T>
         Task<int> thirdTask = Task.Factory.StartNew(() =>
         {
            int sum = 0;
            for (int i = 0; i < 100; i++)
            {
               sum += i;
            }

            return sum;
         });

         Console.WriteLine("Result 3: {0}", thirdTask.Result);

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}