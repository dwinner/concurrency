/**
 * Асинхронные вызовы методов
 */

using System;
using System.Threading;

namespace _17_AsyncCallback
{
   class EntryPoint
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

      static void Main()
      {
         // Выполним асинхронный вызов, создав делегат и вызвав его
         ComputeTaxesDelegate work = ComputeTaxes;
         IAsyncResult pendingOp = work.BeginInvoke(2004, null, null);

         // Выпонить другую полезную работу
         Thread.Sleep(3000);

         // Завершить асинхронный вызов
         Console.WriteLine("Ожидание завершения операции");
         decimal result = work.EndInvoke(pendingOp);
         Console.WriteLine("Сумма налогов: {0}", result);

         Console.ReadKey();
      }
   }
}
