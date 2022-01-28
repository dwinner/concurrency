/**
 * Комбинация параллельных и последовательных циклов
 */

using System;
using System.Threading.Tasks;

namespace _12_MixingSyncAndParallelLoops
{
   internal static class Program
   {
      private static void Main()
      {
         var rnd = new Random();

         const int itemsPerMonth = 100000;

         var sourceData = new BankTransaction[12 * itemsPerMonth];
         for (var i = 0; i < 12 * itemsPerMonth; i++)
         {
            sourceData[i] = new BankTransaction
            {
               Amount = rnd.Next(-400, 500)
            };
         }

         var monthlyBalances = new int[12];

         for (var currentMonth = 0; currentMonth < 12; currentMonth++)
         {
            // Используем параллельный цикл для текущих данных по месяцу
            var month = currentMonth;
            Parallel.For(
               currentMonth * itemsPerMonth,
               (currentMonth + 1) * itemsPerMonth,
               new ParallelOptions(),
               () => 0,
               (index, loopState, tlsBalance) =>
               {
                  tlsBalance += sourceData[index].Amount;
                  return tlsBalance;
               },
               tlsBalance => monthlyBalances[month] += tlsBalance);  // NOTE: Data race?!

            // Теперь можно посчитать баланс текущего месяца на основе предыдущего
            if (currentMonth > 0)
            {
               monthlyBalances[currentMonth] += monthlyBalances[currentMonth - 1];
            }
         }

         for (var i = 0; i < monthlyBalances.Length; i++)
         {
            Console.WriteLine("Month {0} - Balance: {1}", i, monthlyBalances[i]);
         }
      }
   }

   internal class BankTransaction
   {
      public int Amount { get; set; }
   }
}