/**
 * Как уберечь себя от сюрпризов оптимизации и кэширования в буфере процессора
 */

using System;
using System.IO;
using System.Threading;

namespace ThreadSharingData
{
   internal sealed class Program
   {
      private /* Note: а еще можно пометить его как volatile*/ int _flag;
      private int _value;

      private const int WritersCount = 1000;
      private const int ReadersCount = 1000;

      // Этот метод исполняется одним потоком
      public void Thread1()
      {
         // Note: 5 нужно писать в _value до записи 1 в _flag
         _value = 5;
         Thread.VolatileWrite(ref _flag, 1);
      }

      // Этот метод исполнятеся другим потоком
      public void Thread2(object o)
      {
         // Note: поле _value должно быть прочитано после _flag
         if (Thread.VolatileRead(ref _flag) == 1)
         {
            Console.WriteLine(_value);
         }
         else
         {
            Console.WriteLine("BUG FOUND");
         }
      }

      static void Main()
      {
         Console.SetOut(new StreamWriter("Out.txt"));
         var program = new Program();

         for (int i = 0; i < WritersCount; i++)
         {
            var writerThread = new Thread(program.Thread1);
            writerThread.Start();
         }

         for (int i = 0; i < ReadersCount; i++)
         {
            var readerThread = new Thread(program.Thread2);
            readerThread.Start();
         }
         Console.ReadKey();
      }
   }
}
