/**
 * Отмена задач продолжения
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _07_CancellingContinuations
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         // Создаем первичную задачу
         var primaryTask = new Task(() =>
         {
            Console.WriteLine("Antecedent running");
            tokenSource.Token.WaitHandle.WaitOne();
            tokenSource.Token.ThrowIfCancellationRequested();
         }, tokenSource.Token);

         // Создаем выборочное продолжение
         primaryTask.ContinueWith(antecedentTask => Console.WriteLine("This task will never be scheduled"), tokenSource.Token);

         // Создаем "плохое" продолжение при отмене операции
         primaryTask.ContinueWith(antecedentTask => Console.WriteLine("This task will never be scheduled"), tokenSource.Token,
            TaskContinuationOptions.OnlyOnCanceled, TaskScheduler.Current);

         // Создаем "хорошее" продолжение при отмене операции
         Task continuation = primaryTask.ContinueWith(antecedentTask => Console.WriteLine("Continuation running"),
            TaskContinuationOptions.OnlyOnCanceled);

         primaryTask.Start();

         Console.WriteLine("Press enter to cancel token");
         Console.ReadLine();
         tokenSource.Cancel();

         continuation.Wait(/*tokenSource.Token*/);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}