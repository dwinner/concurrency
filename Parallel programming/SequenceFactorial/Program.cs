/**
 * Последовательное вычисление факториалов
 */

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;

namespace _23_SequenceFactorial
{
   class Program
   {
      const int FactorialsToCompute = 2000;

      static void Main()
      {
         var numbers = new ConcurrentDictionary<BigInteger, BigInteger>(4, FactorialsToCompute);

         // Создать делегат для вычисления факториала
         Func<BigInteger, BigInteger> factorial = null;
         factorial = n => (n == 0) ? 1 : n * factorial(n - 1);

         // Вычислить факториалы для списка значений
         var stopwatch = new Stopwatch();
         stopwatch.Start();
         for (ulong i = 0; i < FactorialsToCompute; i++)
         {
            numbers[i] = factorial(i);
            Console.WriteLine("{0} iteration", i);
         }
         stopwatch.Stop();
         Console.WriteLine("Elapsed time:\t{0}", stopwatch.ElapsedMilliseconds); // примерно 80 сек.

         Console.ReadKey();
      }
   }
}
