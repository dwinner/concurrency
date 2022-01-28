/**
 * Блокировки чтения/записи
 */

using System;
using System.Threading;

namespace ReaderWriterLockSlimSample
{
   class Program
   {
      static void Main()
      {
      }
   }

   internal sealed class Transaction : IDisposable
   {
      private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      private DateTime _lastTransactiondaDateTime;

      public void PerformTransaction()
      {
         _readerWriterLock.EnterWriteLock();
         _lastTransactiondaDateTime = DateTime.Now;   // Этот код имеет эксклюзивный доступ к данным...
         _readerWriterLock.ExitWriteLock();
      }

      public DateTime LastTransaction
      {
         get
         {
            _readerWriterLock.EnterReadLock();
            DateTime temp = _lastTransactiondaDateTime;  // Этот код имеет совместный доступ к данным...
            _readerWriterLock.ExitWriteLock();

            return temp;
         }
      }

      public void Dispose()
      {
         _readerWriterLock.Dispose();
      }
   }
}
