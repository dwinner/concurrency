/**
 * Коллекции, безопасные к потокам
 */

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentSample
{
   static class Program
   {
      static void Main()
      {
         Console.WriteLine("Blocking Demo Simple:");
         BlockingDemoSimple();
         Console.Write("Press any key to continue...");
         Console.ReadKey();

         Console.WriteLine("Another blocking Demo:");
         BlockingDemo();
      }

      private static void BlockingDemoSimple()
      {
         var sharedCollection = new BlockingCollection<int>(); // Разделяемый контейнер
         var events = new ManualResetEventSlim[2]; // Сигнальные события
         var waits = new WaitHandle[2];
         for (int i = 0; i < 2; i++)
         {
            events[i] = new ManualResetEventSlim(false);
            waits[i] = events[i].WaitHandle;
         }

         // Поток-поставщик
         var producer = new Thread(obj =>
         {
            var state = (Tuple<BlockingCollection<int>, ManualResetEventSlim>)obj;
            var col1 = state.Item1;
            var ev = state.Item2;
            var r = new Random();

            for (int i = 0; i < 300; i++)
            {
               col1.Add(r.Next(3000));
            }
            ev.Set();
         });
         producer.Start(Tuple.Create(sharedCollection, events[0]));

         // Поток-потребитель
         var consumer = new Thread(obj =>
         {
            var state = (Tuple<BlockingCollection<int>, ManualResetEventSlim>)obj;
            var col1 = state.Item1;
            var ev = state.Item2;

            for (int i = 0; i < 300; i++)
            {
               int result = col1.Take();
            }
            ev.Set();
         });
         consumer.Start(Tuple.Create(sharedCollection, events[1]));

         // Ждем сигнала от всех событий
         Console.WriteLine(!WaitHandle.WaitAll(waits) ? "Wait failed" : "Reading/Writing finished");
      }

      private static void BlockingDemo()
      {
         const int threadCount = 10;
         var events = new ManualResetEventSlim[threadCount];
         var waits = new WaitHandle[threadCount];
         var consoleLock = new object();

         for (int thread = 0; thread < threadCount; thread++)
         {
            events[thread] = new ManualResetEventSlim(false);
            waits[thread] = events[thread].WaitHandle;
         }

         var sharedCollection = new BlockingCollection<int>();

         const int fastDivideByTwo = threadCount >> 1;
         for (int thread = 0; thread < fastDivideByTwo; thread++)
         {
            var producer = new Thread(o =>
            {
               var stateTuple = (Tuple<BlockingCollection<int>, ManualResetEventSlim>)o;
               var col1 = stateTuple.Item1;
               var wait = stateTuple.Item2;
               var r = new Random();
               for (int i = 0; i < 300; i++)
               {
                  int data = r.Next(30000);
                  if (!col1.TryAdd(data))
                  {
                     Console.WriteLine("**** Couldn't Add");
                  }
                  else
                  {
                     lock (consoleLock)
                     {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(" {0} ", data);
                        Console.ResetColor();
                     }
                  }
                  Thread.Sleep(r.Next(40));
               }
               wait.Set();
            });
            producer.Start(Tuple.Create(sharedCollection, events[thread]));
         }

         Thread.Sleep(500);   // Даем фору потокам-поставщикам            

         const int fastDivider = threadCount >> 1;
         for (int thread = fastDivider; thread < threadCount; thread++)
         {
            var consumer = new Thread(o =>
            {
               var stateTuple = (Tuple<BlockingCollection<int>, ManualResetEventSlim>)o;
               var col1 = stateTuple.Item1;
               var wait = stateTuple.Item2;
               var r = new Random();
               for (int i = 0; i < 3000; i++)
               {
                  int result;
                  if (!col1.TryTake(out result))
                  {
                     Console.WriteLine("Couldn't take");
                  }
                  else
                  {
                     lock (consoleLock)
                     {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" {0} ", result);
                        Console.ResetColor();
                     }
                  }

                  Thread.Sleep(r.Next(40));
               }

               wait.Set();
            });
            consumer.Start(Tuple.Create(sharedCollection, events[thread]));
         }

         if (!WaitHandle.WaitAll(waits))
         {
            Console.WriteLine("Error waiting...");
         }
      }
   }
}
