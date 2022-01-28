/**
 * Фоновые и активные потоки
 */

using System;
using System.Threading;

namespace _02_BackgroundAndActiveThreads
{
   public static class Program
   {
      static void Main()
      {
         // Создание нового потока и превращение в фоновый (по умолчанию активного)
         var thread = new Thread(Worker) { IsBackground = true };

         thread.Start();   // Старт потока
         // В случае активного потока приложение будет работать около 10 секунд
         // В случае фонового потока приложение немедленно прекратит работу
         Console.WriteLine("Возвращаюсь из Main");
      }

      private static void Worker()
      {
         Thread.Sleep(10000); // Имитация 10 секунд работы

         // Следующая строка выводится только для кода,
         // исполняемого активным потоком
         Console.WriteLine("Returning from Worker");
      }
   }
}
