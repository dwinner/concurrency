/**
 * Блокировки в обратном порядке приводят к deadlock'ам
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LockAcquisitionOrder
{
   internal static class Program
   {
      private static void Main()
      {
         var lock1 = new object();
         var lock2 = new object();

         var task1 = new Task(() =>
         {
            lock (lock1)
            {
               Console.WriteLine("Task 1 acquired lock 1");
               Thread.Sleep(500);
               lock (lock2)
               {
                  Console.WriteLine("Task 1 acquired lock 2");
               }
            }
         });

         var task2 = new Task(() =>
         {
            lock (lock2)
            {
               Console.WriteLine("Task 2 acquired lock 2");
               Thread.Sleep(500);
               lock (lock1)
               {
                  Console.WriteLine("Task 2 acquired lock 1");
               }
            }
         });

         task1.Start();
         task2.Start();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}