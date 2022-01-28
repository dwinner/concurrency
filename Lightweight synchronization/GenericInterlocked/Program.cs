/**
 * Универсальный шаблон класса Interlocked
 */

using System;
using System.Threading;

namespace GenericInterlocked
{
   internal static class Program
   {
      private static void Main()
      {
         var target = 5;
         const int value = 10;

         var maximum = Maximum(ref target, value);
         Console.WriteLine(maximum);

         Console.WriteLine();

         const int startValue = 7;
         var morph = Morph(ref target, startValue, (int i, int argument, out int morphResult) =>
         {
            var min = Math.Min(i, argument);
            morphResult = min;
            return min;
         });
         Console.WriteLine(morph);
      }

      private static int Maximum(ref int target, int value)
      {
         int currentVal = target, startVal, desiredVal;

         // Параметр target может использоваться другим потоком,
         // его трогать не стоит
         do
         {
            // Запись начального значения этой итерации
            startVal = currentVal;

            // Вычисление желаемого значения в терминах startVal и value
            desiredVal = Math.Max(startVal, value);

            // Note: Здесь поток может быть прерван!

            // Возвращение значения, предшествующего потенциальным изменениям
            currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);

            // Если начальное значение на этой итерации изменилось, повторить
         } while (startVal != currentVal);

         // Возвращаем максимальное значение, когда поток пытается его присвоить
         return desiredVal;
      }

      private static TResult Morph<TResult, TArgument>(ref int target, TArgument argument,
         Morpher<TResult, TArgument> morpher)
      {
         TResult morphResult;
         int currentVal = target, startVal;

         do
         {
            startVal = currentVal;
            var desiredVal = morpher(startVal, argument, out morphResult);
            currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
         } while (startVal != currentVal);

         return morphResult;
      }

      private delegate int Morpher<TResult, in TArgument>(int startValue, TArgument argument, out TResult morphResult);
   }
}