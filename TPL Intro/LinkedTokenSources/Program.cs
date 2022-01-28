/**
 * Связанные токены отмены
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedTokenSources
{
   internal static class Program
   {
      private static void Main()
      {
         var tokenSource1 = new CancellationTokenSource();
         var tokenSource2 = new CancellationTokenSource();
         var tokenSource3 = new CancellationTokenSource();

         CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            tokenSource1.Token, tokenSource2.Token, tokenSource3.Token);

         var task = new Task(() =>
         {
            linkedTokenSource.Token.WaitHandle.WaitOne();
            throw new OperationCanceledException(linkedTokenSource.Token);
         }, linkedTokenSource.Token);

         task.Start();
         tokenSource2.Cancel();
         Console.WriteLine("Main method complete. Press enter to finish");
         Console.ReadLine();
      }
   }
}