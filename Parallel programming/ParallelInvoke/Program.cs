/**
 * Простой вход в пул потоков
 */

using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace _25_ParallelInvoke
{
   class Program
   {
      const int FactorialsToCompute = 100;

      static void Main()
      {
         var numbers = new ConcurrentDictionary<BigInteger, BigInteger>();

         // Создать делегат для вычисления факториала         
         Func<BigInteger, BigInteger> factorial = null;
         factorial = n => (n == 0) ? 1 : n * factorial(n - 1);

         // Построить массив действий
         var actions = new Action[FactorialsToCompute];
         for (int i = 0; i < FactorialsToCompute; i++)
         {
            int x = i;
            actions[i] = () => numbers[x] = factorial(x);
         }

         // Вычислить значения
         Parallel.Invoke(actions);

         // Вывести результаты
         for (int i = 0; i < FactorialsToCompute; i++)
         {
            Console.WriteLine(numbers[i]);
         }

         Console.ReadKey();
      }
   }
}
