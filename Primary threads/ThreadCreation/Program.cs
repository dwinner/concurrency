/**
 * Процедура создания выделенного потока
 */

using System;
using System.Threading;

namespace _01_ThreadCreation
{
   public static class Program
   {
      static void Main()
      {
         Console.WriteLine(
            "Главный поток: запускаю дочерний поток для выполнения асинхронной операции");
         var dedicatedThread = new Thread(ComputeBoundOp);
         dedicatedThread.Start(5);

         Console.WriteLine("Главный поток: делаю другую работу здесь...");
         Thread.Sleep(10000); // Имитирует другую работу (10 секунд)

         dedicatedThread.Join(); // Ожидание завершения потока
         Console.WriteLine("Нажмите <Enter> для завершения программы");
         Console.ReadLine();
      }

      // Сигнатура метода должна совпадать
      // с сигнатурой делегата ParameterizedThreadStart
      private static void ComputeBoundOp(object state)
      {
         // Метод, выполняемый выделенным потоком
         Console.WriteLine("In ComputeBoundOp: state={0}", state);
         Thread.Sleep(1000);  // Имитирует другую работу (1 секунда)
      }
   }
}
