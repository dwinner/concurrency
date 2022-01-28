/**
 * Передача контекстной информации в асинхронный метод
 */
using System;
using System.Threading;

namespace _17_AsyncStateContext
{
   class Program
   {
      // Объявление делегата для асинхронного вызова
      private delegate decimal ComputeTaxesDelegate(int year);

      // Метод для вычисления налогов
      private static decimal ComputeTaxes(int year)
      {
         Console.WriteLine("Вычисление налогов в потоке {0}",
                          Thread.CurrentThread.ManagedThreadId);
         // Здесь происходит длительное вычисление
         Thread.Sleep(6000);
         return 4356.98M;
      }

      private static void TaxesComputed(IAsyncResult ar)
      {
         // Теперь получим результат
         var work = (ComputeTaxesDelegate)ar.AsyncState;
         decimal result = work.EndInvoke(ar);
         Console.WriteLine("Сумма налогов: {0}", result);
      }

      public static void Main(string[] args)
      {
         // Выполним асинхронный вызов, создав делегат и вызвав его.
         var work = new ComputeTaxesDelegate(ComputeTaxes);
         work.BeginInvoke(2004, TaxesComputed, work);

         // Выполнить другую полезную работу
         Thread.Sleep(3000);

         // Завершить асинхронный вызов
         Console.WriteLine("Ожидание завершения операции");

         // Sleep используется только для примера!
         // В действительности необходимо ожидать событие,
         // чтобы получить результат или что-то подобное
         Thread.Sleep(4000);

         Console.Write("Press any key to continue . . . ");
         Console.ReadKey(true);
      }
   }
}