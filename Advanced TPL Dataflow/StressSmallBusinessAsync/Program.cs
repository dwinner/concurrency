using System;
using System.Threading;
using System.Threading.Tasks;
using SharingState.Lib;

// ReSharper disable FunctionNeverReturns

namespace StressSmallBusinessAsync
{
   internal static class Program
   {
      private static void Main()
      {
         var acme = new SmallBusinessAsync();

         acme.RaisedInvoiceForAsync(1000).Wait();
         Task.Run(async () =>
         {
            while (true)
            {
               if (await acme.NetWorthAsync.ConfigureAwait(false) != 1000)
                  Console.WriteLine("Corruption");
            }
         });

         for (var i = 0; i < 1000; i++)
         {
            acme.ReceivePaymentAsync(1);
            Thread.Sleep(10);
         }
      }
   }
}