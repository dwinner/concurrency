/**
 * Напрасное расходование ресурсов
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExcessiveSpinning
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         Task task1 = Task.Factory.StartNew(() =>
         {
            Console.WriteLine("Task 1 waiting for cancellation");
            tokenSource.Token.WaitHandle.WaitOne();
            Console.WriteLine("Task 1 cancelled");
            tokenSource.Token.ThrowIfCancellationRequested();
         }, tokenSource.Token);

         Task.Factory.StartNew(() =>
         {
            while (!task1.Status.HasFlag(TaskStatus.Canceled))
            {
               // NOTE: Расходуем ресурсы
            }
            Console.WriteLine("Task 2 exited code loop");
         }, tokenSource.Token);

         Task.Factory.StartNew(() =>
         {
            while (task1.Status != TaskStatus.Canceled)
            {
               Thread.SpinWait(1000);  // NOTE: Расходуем рерурсы через 1000 тактов процессора
            }
            Console.WriteLine("Task 3 exited spin wait loop");
         }, tokenSource.Token);

         Console.WriteLine("Press enter to cancel token");
         Console.ReadLine();
         tokenSource.Cancel();

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}