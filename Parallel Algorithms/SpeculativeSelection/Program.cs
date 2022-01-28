/**
 * Спекулятивный выбор
 */

using System;
using System.Threading;

namespace SpeculativeSelection
{
   static class Program
   {
      static void Main()
      {
         // Создадим несколько одинаковых функций
         Func<int, double> firstFunc = value =>
         {
            var random = new Random();
            Thread.Sleep(random.Next(1, 2000));
            return Math.Pow(value, 2);
         };

         Func<int, double> secondFunc = value =>
         {
            var random = new Random();
            Thread.Sleep(random.Next(1, 1000));
            return Math.Pow(value, 2);
         };

         // Определим обратный вызов при получении результатов
         Action<long, double> callback =
            (index, result) => Console.WriteLine("Received result of {0} from function {1}", result, index);

         // Вычислим несколько значений в спекулятивном режиме
         for (int i = 0; i < 10; i++)
         {
            SpeculativeSelection.Compute(i, callback, firstFunc, secondFunc);
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}
