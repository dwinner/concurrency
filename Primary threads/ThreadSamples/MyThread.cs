using System;

namespace ThreadSamples
{
   public class MyThread
   {
      private readonly string _data;

      public MyThread(string data)
      {
         _data = data;
      }

      public void ThreadMain()
      {
         Console.WriteLine("Running in a thread, data: {0}", _data);
      }
   }
}