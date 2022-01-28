/**
 * Проблемы рекурсивных/открытых блокировок
 */

using System;
using System.Threading;

namespace RecursiveBlockProblem
{
   class Program
   {
      static void Main()
      {
         var transaction = new Transaction();
         Monitor.Enter(transaction);   // Этот поток получает право на открытое блокирование объекта

         /*
          * Заставляем поток пула вывести время LastTransaction
          * Note: Поток пула заблокирован до вызова
          * методом Main() метода Monitor.Exit!
          */

         ThreadPool.QueueUserWorkItem(state => Console.WriteLine(transaction.LastTransaction)); // Note: Deadlock для потока из пула

         // Здесь выполняется какой-то код...

         Monitor.Exit(transaction);
      }
   }

   internal sealed class Transaction
   {
      private DateTime _timeOfLastTransaction;

      public void PerformTransaction()
      {
         Monitor.Enter(this);

         // Этот код имеет эксклюзивный доступ к данным...
         _timeOfLastTransaction = DateTime.Now;

         Monitor.Exit(this);
      }

      public DateTime LastTransaction
      {
         get
         {
            Monitor.Enter(this);

            // Этот код имеет совместный доступ к данным...
            DateTime temp = _timeOfLastTransaction;

            Monitor.Exit(this);

            return temp;
         }
      }
   }
}
