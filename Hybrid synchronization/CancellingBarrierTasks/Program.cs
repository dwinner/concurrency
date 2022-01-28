/**
 * Отмена участников барьера
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _13_CancellingBarrierTasks
{
   internal static class Program
   {
      private static void Main()
      {
         var barrier = new Barrier(2);

         var tokenSource = new CancellationTokenSource();

         Task.Factory.StartNew(() =>
         {
            Console.WriteLine("Good task starting phase 0");
            barrier.SignalAndWait(tokenSource.Token);
            Console.WriteLine("Good task starting phase 1");
            barrier.SignalAndWait(tokenSource.Token);
         }, tokenSource.Token);

         Task.Factory.StartNew(() =>
         {
            Console.WriteLine("Bad task 1 throwing exception");
            throw new Exception();
         }, tokenSource.Token).ContinueWith(task =>
         {
            Console.WriteLine("Cancelling the token");
            tokenSource.Cancel();
         }, TaskContinuationOptions.OnlyOnFaulted);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}