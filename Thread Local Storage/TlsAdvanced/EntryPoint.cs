/**
 * Более сложный способ организации локального хранилища потока
 */

using System;
using System.Threading;

namespace _06_TlsAdvanced
{
   internal static class EntryPoint
   {
      private static void Main()
      {
         var thread1 = new Thread(ThreadFunc);
         var thread2 = new Thread(ThreadFunc);
         thread1.Start();
         thread2.Start();

         Console.ReadKey();
      }

      private static void ThreadFunc()
      {
         Console.WriteLine("Поток {0} запускается...",
            Thread.CurrentThread.ManagedThreadId);
         Console.WriteLine("Локальные данные для этого потока: \"{0}\"",
            TlsClass.TlsSlot);
         Console.WriteLine("Поток {0} завершается",
            Thread.CurrentThread.ManagedThreadId);
      }
   }

   public class TlsClass
   {
      private static readonly LocalDataStoreSlot ThreadLocalSlot;

      static TlsClass()
      {
         ThreadLocalSlot = Thread.AllocateDataSlot();
      }

      private TlsClass()
      {
         Console.WriteLine("Создание TlsClass");
      }

      public static TlsClass TlsSlot
      {
         get
         {
            var obj = Thread.GetData(ThreadLocalSlot);
            if (obj != null)
               return (TlsClass)obj;
            obj = new TlsClass();
            Thread.SetData(ThreadLocalSlot, obj);
            return (TlsClass)obj;
         }
      }
   }
}