/**
 * Модель асинхронного программирования и вычислительные операции
 */

using System;

namespace CanonicalApm
{
   static class Program
   {
      static void Main()
      {
         // Переменной делегата присваивается ссылка на асинхронно вызываемый метод
         Func<UInt64, UInt64> sumDelegate = Sum;

         // Вызов метода при помощи потока из пула
         sumDelegate.BeginInvoke(1000000000, SumIsDone, sumDelegate);

         // Здесь выполняется какой-то другой код

         Console.ReadLine();
      }

      private static void SumIsDone(IAsyncResult ar)
      {
         // Извлекаем sumDelegate (состояние) из объекта IAsyncResult
         var sumDelegate = ar.AsyncState as Func<UInt64, UInt64>;

         if (sumDelegate != null)
         {
            try
            {
               // Получаем и выводим результат
               Console.WriteLine("Sum's result: {0}", sumDelegate.EndInvoke(ar));
            }
            catch (OverflowException overflowException)
            {
               Console.WriteLine(overflowException);
            }
         }
      }

      private static UInt64 Sum(UInt64 n)
      {
         // Note: самый быстрый способ вычислить сумму это sum = n * (n + 1) / 2
         UInt64 sum = 0;
         for (UInt64 i = 1; i <= n; i++)
         {
            checked
            {
               // Я проверяю код: и если сумма не поместиться в структуре UInt64,
               // будет вброшено исключение OverflowException
               sum += i;
            }
         }
         return sum;
      }
   }
}
