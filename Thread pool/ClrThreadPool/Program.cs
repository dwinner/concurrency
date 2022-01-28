/**
 * Вызов метода потоком из пула
 */

using System;
using System.Threading;

namespace _03_ClrThreadPool
{
   public static class Program
   {
      static void Main()
      {
         Console.WriteLine(
            "Главный поток: ставлю в очередь на выполнение асинхронную операцию");
         ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
         Console.WriteLine(
            "Главный поток: делаю другую работу...");
         Thread.Sleep(10000); // Имитация другой работы (10 секунд)
         Console.WriteLine("Нажмите <Enter> для выхода из программы");
         Console.ReadLine();
      }

      /// <summary>
      ///  Сигнатура этого метода совпадает с сигнатурой делегата WaitCallback
      /// </summary>
      /// <param name="state">Объект для передачи состояния</param>
      private static void ComputeBoundOp(object state)
      {
         // Метод выполняется потоком из пула
         Console.WriteLine("В ComputeBoundOp: state={0}", state);
         Thread.Sleep(1000);  // Имитация другой работы (1 секунда)

         // После возвращения управления методом поток
         // возвращается в пул и ожидает следующего задания
      }
   }
}
