/**
 * Непоследовательная или непроверяемая отмена задач
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace InconsistentOrUncheckedCancellation
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         var task1 = new Task<int>(() =>
         {
            tokenSource.Token.WaitHandle.WaitOne(); // Ждем отмены на токене
            tokenSource.Token.ThrowIfCancellationRequested();
            return 100;
         }, tokenSource.Token);

         // Создаем плохую задачу продолжения
         /*Task task2 = */
         task1.ContinueWith(task => Console.WriteLine("Antecedent result: {0}", task.Result));

         // Создаем задачу продолжения, использующую токен
         /*Task task3 = */
         task1.ContinueWith(task => Console.WriteLine("Never be executed"), tokenSource.Token);

         // Создаем задачу продолжения, проверяющую статус предыдущей
         /*Task task4 = */
         task1.ContinueWith(task =>
            {
               if (task.Status == TaskStatus.Canceled)
               {
                  Console.WriteLine("Antecedent cancelled");
               }
               else
               {
                  Console.WriteLine("Antecedent result: {0}", task.Result);
               }
            });

         Console.WriteLine("Press enter to cancel token");
         Console.ReadLine();
         tokenSource.Cancel();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}