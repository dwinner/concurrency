/**
 * Закрытое блокирование
 */

using System;
using System.Threading;

namespace InternalBlocking
{
   static class Program
   {
      static void Main()
      {
      }
   }

   internal sealed class Transaction
   {
      private readonly object _privateLock = new object(); // Теперь блокирование в рамках каждой транзакции закрыто
      private DateTime _timeOfLastTransaction;

      public void PerformTransaction()
      {
         Monitor.Enter(_privateLock);  // Доступ к закрытому блокированию

         // Этот код имеет эксклюзивный доступ к данным...
         _timeOfLastTransaction = DateTime.Now;

         Monitor.Exit(_privateLock);   // Завершаем закрытое блокирование
      }

      public DateTime LastTransaction
      {
         get
         {
            Monitor.Enter(_privateLock);  // Доступ к закрытому блокированию

            // Этот код имеет совместный доступ к данным...
            DateTime temp = _timeOfLastTransaction;

            Monitor.Exit(_privateLock);   // Завершаем закрытое блокирование

            return temp;
         }
      }
   }
}
