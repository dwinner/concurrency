using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal sealed class ReaderWriter : IDisposable
{
   private readonly List<int> _items = new() { 0, 1, 2, 3, 4, 5 };
   private readonly ReaderWriterLockSlim _rwl = new();
   private bool disposedValue;

   void IDisposable.Dispose()
   {
      Dispose(true);
      GC.SuppressFinalize(this);
   }

   public void ReaderMethod(object? reader)
   {
      Console.WriteLine($"Starting reader {reader}");
      try
      {
         _rwl.EnterReadLock();
         for (var i = 0; i < _items.Count; i++)
         {
            Console.WriteLine($"reader {reader}, loop: {i}, item: {_items[i]}");
            Task.Delay(40).Wait();
         }
      }
      finally
      {
         _rwl.ExitReadLock();
      }
   }

   public void WriterMethod(object? writer)
   {
      Console.WriteLine($"Startring writer {writer}");
      try
      {
         while (!_rwl.TryEnterWriteLock(50))
         {
            Console.WriteLine($"Writer {writer} waiting for the write lock, current readers: {_rwl.CurrentReadCount}");
         }

         Console.WriteLine($"Writer {writer} acquired the lock");
         for (var i = 0; i < _items.Count; i++)
         {
            _items[i]++;
            Task.Delay(50).Wait();
         }

         Console.WriteLine($"Writer {writer} finished");
      }
      finally
      {
         _rwl.ExitWriteLock();
      }
   }

   private void Dispose(bool disposing)
   {
      if (!disposedValue)
      {
         if (disposing)
         {
            _rwl.Dispose();
         }

         disposedValue = true;
      }
   }
}