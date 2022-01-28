/**
 * Конвейер
 */

using System;

namespace PipelineSample
{
   static class Program
   {
      static void Main()
      {
         // Создадим набор функций, которые мы хотим обрабатывать вместе
         Func<int, double> func1 = input => Math.Pow(input, 2);
         Func<double, double> func2 = input => input / 2;
         Func<double, bool> func3 = input => Math.Abs(input % 2) < Double.Epsilon && input > 100;

         // Определим обратный вызов
         Action<int, bool> callback = (input, output) =>
         {
            if (output)
            {
               Console.WriteLine("Found value {0} with result {1}", input, true);
            }
         };

         // Создадим конвейер
         Pipeline<int, bool> pipeline = new Pipeline<int, double>(func1)
            .AddFunction(func2)
            .AddFunction(func3);

         // Стартуем конвейер
         pipeline.StartProcessing();

         // Генерируем значения и добавляем их к конвейеру
         for (int i = 0; i < 1000; i++)
         {
            Console.WriteLine("Added value {0}", i);
            pipeline.AddValue(i, callback);
         }

         // Останавливаем конвейер
         pipeline.StopProcessing();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
