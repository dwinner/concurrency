/**
 * Параллельное вычисление факториала
 */

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace _24_ParallelFactorial
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

         // Вычислить факториалы для списка значений параллельно
         var stopwatch = new Stopwatch();
         stopwatch.Start();
         Parallel.For(0,
            FactorialsToCompute,
            i =>
            {
               numbers[i] = factorial(i);
               Console.WriteLine("{0} iteration", i);
            }
         );
         stopwatch.Stop();
         Console.WriteLine("Elapsed time:\t{0}", stopwatch.ElapsedMilliseconds);
         // примерно 310 сек. без gcServer enabled="true" на одном процессоре
         // примерно 292 сек. с gcServer enabled="true" на одном процессоре  
         // На многоядерном процессоре производительность должна увеличится в разы!

         Console.ReadKey();
      }
   }
}
