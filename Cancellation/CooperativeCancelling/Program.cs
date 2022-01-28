/**
 * Скоординированная отмена
 */

using System;
using System.Threading;

namespace _05_CooperativeCancelling
{
   public static class Program
   {
      private static void Main()
      {
         CancellationDemo.Go();
         CancellationTokenRegistrationDemo.Go();

         Console.ReadKey();
      }
   }

   #region Скоординированная отмена

   internal static class CancellationDemo
   {
      public static void Go()
      {
         var cancellationTokenSource = new CancellationTokenSource();

         // Передаем операции CancellationToken и число
         ThreadPool.QueueUserWorkItem(o => Count(cancellationTokenSource.Token, 1000));
         Console.WriteLine("Press <Enter> to cancel the operation.");
         Console.ReadLine();
         cancellationTokenSource.Cancel();
         // NOTE: Если метод Count уже вернул управление, Cancel не оказывает никакого эффекта
         // NOTE: Cancel немедленно возвращает управление, и метод продолжает работу
      }

      private static void Count(CancellationToken token, int countTo)
      {
         for (var count = 0; count < countTo; count++)
         {
            if (token.IsCancellationRequested)
            {
               Console.WriteLine("Count is cancelled");
               break; // Выход из цикла для остановки операции
            }

            Console.WriteLine(count);
            Thread.Sleep(200); // Для демонстрационных целей просто ждем
         }
         Console.WriteLine("Count is done");
      }
   }

   #endregion

   #region Обратные вызовы при скоординированной отмене операций

   internal static class CancellationTokenRegistrationDemo
   {
      public static void Go()
      {
         // Создание объекта CancellationTokenSource
         var firstCts = new CancellationTokenSource();
         firstCts.Token.Register(() => Console.WriteLine("Cts1 canceled"));

         // Создание второго объекта CancellationTokenSource
         var secondCts = new CancellationTokenSource();
         secondCts.Token.Register(() => Console.WriteLine("Cts2 canceled"));

         // Создание нового объекта CancellationTokenSource, отменяемого при
         // отмене cts1 или cts2
         var linkedTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(firstCts.Token, secondCts.Token);

         // Отмена одного из объектов CancellationTokenSource
         secondCts.Cancel();

         // Показываем, какой из объектов CancellationTokenSource был отменен
         Console.WriteLine("cts1 canceled={0}, cts2 canceled={1}, linkedCts={2}",
            firstCts.IsCancellationRequested, // false
            secondCts.IsCancellationRequested, // true
            linkedTokenSource.IsCancellationRequested); // true
      }
   }

   #endregion
}