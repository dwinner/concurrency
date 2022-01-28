/**
 * Строки кэша и ложное разделение
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace _15_FalseSharing
{
   public static class EntryPoint
   {
      static void Main()
      {
         FalseSharing.Go();   // Из-за постоянного обмена кэша с ОП, работает медленнее
         TrueSharing.Go();    // Отработает в 4 раза быстрее

         Console.ReadLine();
      }
   }

   #region Ложное разделение

   internal static class FalseSharing
   {
      private class Data
      {
         // Два соседних поля, скорее всего, расположены в одной строке кэша
         // ReSharper disable once NotAccessedField.Local
         public int Field1;
         // ReSharper disable once NotAccessedField.Local
         public int Field2;
      }

      private const int ITERATIONS = 100000000; // 100 миллионов
      private static int _operations = 2;
      private static long _startTime;

      public static void Go()
      {
         // Выделяем объект и записываем начальное время
         var data = new Data();
         _startTime = Stopwatch.GetTimestamp();

         // Два потока имеют доступ к своим полям внутри структуры
         ThreadPool.QueueUserWorkItem(o => AccessData(data, 0));
         ThreadPool.QueueUserWorkItem(o => AccessData(data, 1));

         Console.WriteLine("Press any key to continue after false sharing");
         Console.ReadKey();
      }

      private static void AccessData(Data data, int field)
      {
         // Каждый объект имеет доступ у своим полям в объекте Data
         for (int x = 0; x < ITERATIONS; x++)
         {
            if (field == 0)
            {
               data.Field1++;
            }
            else
            {
               data.Field2++;
            }
         }

         // Последний завершенный поток показывает время работы
         if (Interlocked.Decrement(ref _operations) == 0)
         {
            Console.WriteLine("Access time: {0:N0}", Stopwatch.GetTimestamp() - _startTime);
         }
      }
   }

   #endregion


   #region Разделение полей по разным строкам кэша

   internal static class TrueSharing
   {
      [StructLayout(LayoutKind.Explicit)]
      private class Data
      {
         // Два поля больше не принадлежат одной строке кэша
         // ReSharper disable once NotAccessedField.Local
         [FieldOffset(0)]
         public int field1;

         // ReSharper disable once NotAccessedField.Local
         [FieldOffset(64)]
         public int field2;
      }

      private const int ITERATIONS = 100000000; // 100 миллионов
      private static int _operations = 2;
      private static long _startTime;

      public static void Go()
      {
         // Выделяем объект и записываем начальное время
         var data = new Data();
         _startTime = Stopwatch.GetTimestamp();

         // Два потока имеют доступ к своим полям внутри структуры
         ThreadPool.QueueUserWorkItem(o => AccessData(data, 0));
         ThreadPool.QueueUserWorkItem(o => AccessData(data, 1));

         Console.WriteLine("Press any key to continue after true sharing");
         Console.ReadKey();
      }

      private static void AccessData(Data data, int field)
      {
         // Каждый объект имеет доступ у своим полям в объекте Data
         for (int x = 0; x < ITERATIONS; x++)
         {
            if (field == 0)
            {
               data.field1++;
            }
            else
            {
               data.field2++;
            }
         }

         // Последний завершенный поток показывает время работы
         if (Interlocked.Decrement(ref _operations) == 0)
         {
            Console.WriteLine("Access time: {0:N0}", Stopwatch.GetTimestamp() - _startTime);
         }
      }
   }

   #endregion

}
