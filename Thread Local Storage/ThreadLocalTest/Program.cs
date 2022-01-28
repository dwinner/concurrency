/**
 * Локальное хранилище потока
 */

using System;
using System.Threading;

namespace ThreadLocalTest
{
   class Program
   {
      static readonly ThreadLocal<string> Greeting;
      readonly ThreadLocal<int> _numThreads = new ThreadLocal<int>(() => 1); // instance field

      static Program()
      {
         Greeting = new ThreadLocal<string>(() => "Greetings from the current thread");
      }

      static void Main()
      {
         Console.WriteLine(Greeting.Value.Replace("current", "main"));
         Greeting.Value = "Goodbye from the main thread";
         var thread1 = new Thread(ThreadMethod);
         thread1.Start();
         thread1.Join();
         var program = new Program();
         var thread2 = new Thread(program.InstanceThreadMethod);
         thread2.Start();
         thread2.Join();
         Console.WriteLine("The number of current threads is now {0}", program._numThreads);
         Console.WriteLine(Greeting.Value); // prints the main thread's copy which is still 1
         Console.ReadKey();
      }

      static void ThreadMethod()
      {
         Console.WriteLine(Greeting.Value.Replace("current", "second"));
         Greeting.Value = "Hello from the second thread"; // only affects the second thread's copy
         Console.WriteLine(Greeting.Value);
      }

      void InstanceThreadMethod()
      {
         _numThreads.Value++; // increment this thread's copy to 2
         Console.WriteLine("The number of current threads is {0}", _numThreads);
      }
   }
}
