using System;
using System.Threading;

namespace ProducerConsumerNonSleepyThreads
{
   internal static class Program
   {
      private static void Main()
      {
         var dcq = new LazyProducerConsumerQueue<int>(3, i =>
         {
            Console.WriteLine("{0}:Processing...", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(i);
            Console.WriteLine("{0}:Done", Thread.CurrentThread.ManagedThreadId);
         });

         dcq.Enqueue(1000);
         Thread.Sleep(2000);
         dcq.Enqueue(200);
         Thread.Sleep(1000);

         var rnd = new Random();
         for (var i = 0; i < 10; i++)
            dcq.Enqueue(rnd.Next(1000, 5000));
         Console.ReadLine();
         dcq.Enqueue(2000);
         Thread.Sleep(2000);
         dcq.Enqueue(1000);

         Console.ReadLine();
      }
   }
}