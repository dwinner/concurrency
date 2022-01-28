// Свободное от блокировок обновление и чтение

using System;
using System.Threading.Tasks;
using static System.Console;
using static System.Environment;
using static System.Threading.Tasks.Task;
using static LockFreeViaSpinWait.LockFreestyle;

namespace LockFreeViaSpinWait
{
   internal static class Program
   {
      private const int OperationCount = 1000000;
      private static readonly int ConcurrentTaskCount = ProcessorCount;
      private static readonly int ActualValue = OperationCount * ConcurrentTaskCount;

      private static void Main()
      {
         var monitorDuration = MonitorSample();
         WriteLine("Monitor approach: {0}", monitorDuration);
         WriteLine();
         var lockFreeDuration = LockFreeSample();
         WriteLine("Lock free approach: {0}", lockFreeDuration);
      }

      private static TimeSpan MonitorSample()
      {
         var duration = TimeSpan.Zero;
         var writeLocker = new object();

         using (var autoStopwatch = new AutoStopwatch())
         {
            autoStopwatch.TotalElapsed += (sender, args) => { duration = args.Spent; };

            var account = new BankAccount();
            var updatingTasks = new Task[ConcurrentTaskCount];
            for (var i = 0; i < updatingTasks.Length; i++)
            {
               updatingTasks[i] = new Task(() =>
               {
                  for (var j = 0; j < OperationCount; j++)
                  {
                     lock (writeLocker)
                     {
                        // Длительные вычисления
                        var current = account.Balance;
                        for (var k = 0; k < 10; k++)
                        {
                           var poweredBalance = Math.Pow(current, 2);
                           current = (int)Math.Sqrt(poweredBalance);
                        }

                        account.Balance = account.Balance + 1;
                     }
                  }
               });

               updatingTasks[i].Start();
            }

            WaitAll(updatingTasks);
            WriteLine("Expected value {0}, Counter value: {1}", ActualValue, account.Balance);
         }

         return duration;
      }

      private static TimeSpan LockFreeSample()
      {
         var duration = TimeSpan.Zero;

         using (var autoStopwatch = new AutoStopwatch())
         {
            autoStopwatch.TotalElapsed += (sender, args) => { duration = args.Spent; };

            var account = new BankAccount();
            var updatingTasks = new Task[ConcurrentTaskCount];
            for (var i = 0; i < updatingTasks.Length; i++)
            {
               updatingTasks[i] = new Task(() =>
               {
                  for (var j = 0; j < OperationCount; j++)
                  {
                     LockFreeUpdate(ref account, bankAccount =>
                     {
                        // Длительные вычисления
                        var current = bankAccount.Balance;
                        for (var k = 0; k < 10; k++)
                        {
                           var poweredBalance = Math.Pow(current, 2);
                           current = (int)Math.Sqrt(poweredBalance);
                        }

                        return new BankAccount { Balance = bankAccount.Balance + 1 };
                     });
                  }
               });

               updatingTasks[i].Start();
            }

            WaitAll(updatingTasks);
            WriteLine("Expected value {0}, Counter value: {1}", ActualValue, account.Balance);
         }

         return duration;
      }
   }
}