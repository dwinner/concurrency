/**
 * Прерывание потоков
 */

using System;
using System.Threading;

namespace _04_Abort
{
   class Program
   {
      static void Main()
      {
         var newThread = new Thread(ThreadFunc);
         newThread.Start();
         Thread.Sleep(2000);

         // Прервать поток
         newThread.Abort();

         // Ждать завершения потока
         newThread.Join();

         Console.ReadKey();
      }

      private static void ThreadFunc()
      {
         ulong counter = 0;
         while (true)
         {
            try
            {
               Console.WriteLine("{0}", counter++);
            }
            catch (ThreadAbortException)    // Выход из "вечности"
            {
               // Попытка проглотить исключение и продолжиться
               Console.WriteLine("Abort and continue");
            }
         }
      }
   }
}
