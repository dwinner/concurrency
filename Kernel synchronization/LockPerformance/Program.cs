/**
 * Сравнение производительности при различных типах блокировок
 */

using SimpleWaitLockViaAutoResetEvent;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LockPerformance
{
   static class Program
   {
      static void Main()
      {
#pragma warning disable 219
         int x = 0;
#pragma warning restore 219
         const int iterations = 10000000; // 10 миллионов

         // Note: Сколько времени займет инкремент x 10 миллионов раз?
         Stopwatch stopwatch = Stopwatch.StartNew();
         for (int i = 0; i < iterations; i++)
         {
            x++;
         }
         Console.WriteLine("Incrementing x: {0:N0}", stopwatch.ElapsedMilliseconds);

         // Note: Сколько времени займет инкремент x 10 миллионов раз, если
         // добавить вызов ничего не делающего метода?
         stopwatch.Restart();
         for (int i = 0; i < iterations; i++)
         {
            Dummy();
            x++;
            Dummy();
         }
         Console.WriteLine("Incrementing x in Dummy: {0:N0}", stopwatch.ElapsedMilliseconds);

         // Note: Сколько времени займет инкремент x 10 миллионов раз, если
         // добавить вызов неконкурирующего объекта SpinLock
         var spinLock = new SpinLock();
         stopwatch.Restart();
         for (int i = 0; i < iterations; i++)
         {
            bool lockTaken = false;
            spinLock.Enter(ref lockTaken);
            x++;
            spinLock.Exit();
         }
         Console.WriteLine("Incrementing x in SpinLock: {0:N0}", stopwatch.ElapsedMilliseconds);

         // Note: Сколько времени займет инкремент x 10 миллионов раз, если
         // добавить вызов неконкурирующего объекта SimpleWaitLock?
         using (var waitLock = new SimpleWaitLock())
         {
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
               waitLock.Enter();
               x++;
               waitLock.Leave();
            }
            Console.WriteLine("Incrementing x in SimpleWaitLock: {0:N0}", stopwatch.ElapsedMilliseconds);
         }
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      private static void Dummy()
      {
         /* Этот метод только возвращает управление */
      }
   }
}
