/**
 * Проверка статуса порождающей задачи
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AssumingStatus
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource = new CancellationTokenSource();

         var tasks = new Task<int>[2];

         tasks[0] = new Task<int>(() =>
         {
            while (true)
            {
               Thread.Sleep(100000);
            }
         });

         tasks[1] = new Task<int>(() =>
         {
            tokenSource.Token.WaitHandle.WaitOne(); // Ждем отмены
            tokenSource.Token.ThrowIfCancellationRequested();
            return 200;
         }, tokenSource.Token);

         // NOTE: Это не приведет к необработанному исключению, задачи продолжения просто не будет
         Task.Factory.ContinueWhenAny(tasks, task => Console.WriteLine("Result of antecedent is {0}", task.Result));

         foreach (var task in tasks) task.Start();

         Console.WriteLine("Press enter to cancel token");
         Console.ReadLine();
         tokenSource.Cancel();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}