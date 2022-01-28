/**
 * Квитирование между потоками
 */

using System;
using System.Threading;

namespace _13_HandShaking
{
   class EntryPoint
   {
      private static int _counter;
      private static readonly object theLock = new object();

      private static void ThreadFunc1()
      {
         lock (theLock)
         {
            for (int i = 0; i < 50; i++)
            {
               Monitor.Wait(theLock, Timeout.Infinite);
               Console.WriteLine("{0} из потока {1}",
                   ++_counter,
                   Thread.CurrentThread.ManagedThreadId);
               Monitor.Pulse(theLock);
            }
         }
      }

      private static void ThreadFunc2()
      {
         lock (theLock)
         {
            for (int i = 0; i < 50; i++)
            {
               Monitor.Pulse(theLock);
               Monitor.Wait(theLock, Timeout.Infinite);
               Console.WriteLine("{0} из потока {1}",
                   ++_counter,
                   Thread.CurrentThread.ManagedThreadId);
            }
         }
      }

      static void Main()
      {
         var thread1 = new Thread(ThreadFunc1);
         var thread2 = new Thread(ThreadFunc2);
         thread1.Start();
         thread2.Start();

         Console.ReadKey();
      }
   }
}
