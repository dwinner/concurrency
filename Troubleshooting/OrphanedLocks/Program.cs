/**
 * Неосвобождаемые блокировки
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrphanedLocks
{
   internal static class Program
   {
      private static void Main()
      {
         var mutex = new Mutex();

         var tokenSource = new CancellationTokenSource();

         var task1 = new Task(() =>
         {
            while (true)
            {
               mutex.WaitOne();
               Console.WriteLine("Task 1 acquired mutex");
               tokenSource.Token.WaitHandle.WaitOne(500);
               mutex.ReleaseMutex();
               Console.WriteLine("Task 1 released mutex");
            }
         }, tokenSource.Token);

         var task2 = new Task(() =>
         {
            tokenSource.Token.WaitHandle.WaitOne(2000);
            mutex.WaitOne();
            Console.WriteLine("Task 2 acquired mutex");
            throw new Exception("Abandoning Mutex");
         }, tokenSource.Token);

         task1.Start();
         task2.Start();

         tokenSource.Token.WaitHandle.WaitOne(3000);

         try
         {
            task2.Wait(tokenSource.Token);
         }
         catch (AggregateException ex)
         {
            ex.Handle(inner =>
            {
               Console.WriteLine(inner);
               return true;
            });
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}