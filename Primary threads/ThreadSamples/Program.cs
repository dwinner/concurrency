/**
 * Класс Thread
 */

using System;
using System.Threading;

namespace ThreadSamples
{
   class Program
   {
      static void Main()
      {
         //FirstThread();
         //PassParameters();         
         //PassParameters2();
         TestThreadPriorities();
      }

      private static void FirstThread()  // NOTE: Простейший способ создания и запуска потока
      {
         new Thread(() => Console.WriteLine("Running in a thread, id: {0}",
            Thread.CurrentThread.ManagedThreadId)).Start();
         Console.WriteLine("This is the main thread, id: {0}", Thread.CurrentThread.ManagedThreadId);
      }

      private static void Background() // NOTE: Фоновый поток
      {
         var t1 = new Thread(() => Console.WriteLine(Thread.CurrentThread.ManagedThreadId))
         {
            Name = "MyNewThread1",
            IsBackground = true
         };
         t1.Start();
         Console.WriteLine("Main thread ending now...");
      }

      #region Способы передачи данных в потоки выполнения

      private static void PassParameters()
      {
         var d = new Data { Message = "Info" };
         var t2 = new Thread(ThreadMainWithParameters);
         t2.Start(d);
      }

      private static void ThreadMainWithParameters(object o)
      {
         var d = (Data)o;
         Console.WriteLine("Running in a thread, received {0}", d.Message);
      }

      private static void PassParameters2()
      {
         var obj = new MyThread("info");
         var t3 = new Thread(obj.ThreadMain);
         t3.Start();
      }

      #endregion

      #region Приоритеты потоков

      static void TestThreadPriorities()
      {
         var t1 = new Thread(Prio) { Name = "First", Priority = ThreadPriority.Highest };
         var t2 = new Thread(Prio) { Name = "Second", Priority = ThreadPriority.Lowest };
         t1.Start();
         t2.Start();
      }

      static void Prio()
      {
         for (int i = 0; i < 10000; i++)
         {
            Console.WriteLine("{0}, {1}", Thread.CurrentThread.Name, i);
         }
      }

      #endregion
   }
}
