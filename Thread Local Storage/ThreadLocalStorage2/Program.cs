/**
 * Локальное хранилище потока
 */

using System;
using System.Threading;

namespace _05_ThreadLocalStorage
{
   class Program
   {
      static void Main()
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
         Console.WriteLine("tlsdata для этого потока - \"{0}\"",
             TlsFieldClass.TlsData);
         Console.WriteLine("Поток {0} завершается",
             Thread.CurrentThread.ManagedThreadId);
      }
   }

   public class TlsClass
   {
      public TlsClass()
      {
         Console.WriteLine("Создание TLSClass");
      }
   }

   public class TlsFieldClass
   {
      [ThreadStatic]
      public static TlsClass TlsData = new TlsClass();    // Note: ловушка
   }
}
