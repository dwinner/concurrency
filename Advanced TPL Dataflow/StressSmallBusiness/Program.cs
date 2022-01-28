using System;
using System.Threading;
using System.Threading.Tasks;
using SharingState.Lib;

namespace StressSmallBusiness
{
   internal static class Program
   {
      private static void Main()
      {
         SmallBusiness acme = new SmallBusinessReadWriteLock();
         acme.RaisedInvoiceFor(1000);
         /*var audit = */
         Task.Run(() =>
         {
            while (true)
               if (acme.NetWorth != 1000)
                  Console.WriteLine("Corruption");
            // ReSharper disable FunctionNeverReturns
         });
         // ReSharper restore FunctionNeverReturns

         for (var i = 0; i < 1000; i++)
         {
            acme.ReceivePayments(1);
            Thread.Sleep(10);
         }
      }
   }
}