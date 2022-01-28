/**
 * Асинхронные делегаты и класс WaitHandle
 */

using System;
using System.Diagnostics;
using System.Threading;

namespace AsyncDelegate
{
   class Program
   {
      static void Main()
      {
         WaitHandlePooling();
         Pooling();
         AsyncCallbackSample();

         Console.ReadLine();
      }

      public delegate int TakesaWhileDelegate(int data, int ms);

      private static int TakesaWhile(int data, int ms)
      {
         Console.WriteLine("TakesaWhile started");
         Thread.Sleep(ms);
         Console.WriteLine("TakesaWhile completed");

         return ++data;
      }

      #region Опрос через дескриптор ожидания

      private static void WaitHandlePooling()
      {
         TakesaWhileDelegate whileDelegate = TakesaWhile;

         IAsyncResult asyncResult = whileDelegate.BeginInvoke(1, 3000, null, null);
         while (true)
         {
            Console.Write(".");
            if (asyncResult.AsyncWaitHandle.WaitOne(50, false))
            {
               Console.WriteLine("Can get the result now");
               break;
            }
         }

         int result = whileDelegate.EndInvoke(asyncResult);
         Console.WriteLine("result: {0}", result);
      }

      #endregion

      #region Опрос

      private static void Pooling()
      {
         TakesaWhileDelegate takesaWhileDelegate = TakesaWhile;

         IAsyncResult asyncResult = takesaWhileDelegate.BeginInvoke(1, 3000, null, null);
         while (!asyncResult.IsCompleted)
         {
            Console.Write(".");
            Thread.Sleep(50);
         }

         int result = takesaWhileDelegate.EndInvoke(asyncResult);
         Console.WriteLine("result: {0}", result);
      }

      #endregion

      #region Обратный вызов при завершении асинхронной операции

      private static void AsyncCallbackSample()
      {
         TakesaWhileDelegate takesaWhile = TakesaWhile;
         takesaWhile.BeginInvoke(1, 3000, TakesaWhileCompleted, takesaWhile);
         // NOTE: Или так:
         //takesaWhile.BeginInvoke(1, 3000, ar =>
         //{
         //   int result = takesaWhile.EndInvoke(ar);
         //   Console.WriteLine("result: {0}", result);
         //}, null);
      }

      private static void TakesaWhileCompleted(IAsyncResult asyncResult)
      {
         if (asyncResult == null)
         {
            throw new ArgumentNullException("asyncResult");
         }

         var takesaWhileDelegate = asyncResult.AsyncState as TakesaWhileDelegate;
         Trace.Assert(takesaWhileDelegate != null, "Invalid object type");

         int result = takesaWhileDelegate.EndInvoke(asyncResult);
         Console.WriteLine("result: {0}", result);
      }

      #endregion

   }
}
