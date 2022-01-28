/**
 * Асинхронный вызов метода
 */

using System;
using System.Threading;

namespace AsyncMethod
{
   class Program
   {
      // Асинхронные вызовы методов нужно делать с помощью делегатов
      delegate double DoWorkDelegate(int maxValue);

      static void Main()
      {
         DoWorkDelegate del = DoWork;

         // Note: У вас есть два способа получить уведомление об окончании метода:

         // Note: 1) Через метод обратного вызова      

         IAsyncResult asyncResult = del.BeginInvoke(100000000,
            stateObject => Console.WriteLine("Computation done"), // Фактически никакое состояние передано не было
            null);
         for (int i = 0; i < 5; i++)
         {
            Console.WriteLine("Doing other work...{0}", i);
            Thread.Sleep(1000);
         }

         // Note: 2) С помощью EndInvoke

         double sum = del.EndInvoke(asyncResult);  // Дождаться окончания
         Console.WriteLine("Sum: {0}", sum);

         Console.ReadKey();
      }

      static double DoWork(int maxValue)
      {
         Console.WriteLine("In DoWork");
         double sum = 0.0;
         for (int i = 1; i < maxValue; ++i)
         {
            sum += Math.Sqrt(i);
         }
         return sum;
      }
   }
}
