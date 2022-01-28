/**
 * Задача продолжения для первой выполнившейся
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _06_AnyOfSeveralToOne
{
   internal static class Program
   {
      private static void Main()
      {
         var tasks = new Task<int>[10];

         var tokenSource = new CancellationTokenSource();

         var rnd = new Random();

         for (int i = 0; i < 10; i++)
         {
            tasks[i] = new Task<int>(() =>
            {
               int sleepInterval;
               lock (rnd)
               {
                  sleepInterval = rnd.Next(500, 2000);
               }

               tokenSource.Token.WaitHandle.WaitOne(sleepInterval);
               tokenSource.Token.ThrowIfCancellationRequested();

               return sleepInterval;
            }, tokenSource.Token);
         }

         Task continueWhenAnyTask = Task.Factory.ContinueWhenAny(tasks,
            antecedent => Console.WriteLine("The first task slept for {0} milliseconds", antecedent.Result));

         foreach (var task in tasks)
         {
            task.Start();
         }

         continueWhenAnyTask.Wait(tokenSource.Token); // Ждем задачу продолжения
         tokenSource.Cancel(); // Отменяем все остальные задачи, незавершившиеся первыми

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}