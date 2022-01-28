/**
 * Защита данных в нескольких процессах
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MutexDemo
{
   class Program
   {
      static void Main()
      {
         Mutex mutex = new Mutex(false, "MutexDemo");
         Process currentProcess = Process.GetCurrentProcess();
         const string outputFile = "MutexDemoOutput.txt";

         while (!Console.KeyAvailable)
         {
            mutex.WaitOne();
            Console.WriteLine("Process {0} gained control",
               currentProcess.Id);
            using (FileStream fileStream = new FileStream(outputFile, FileMode.OpenOrCreate))
            using (TextWriter textWriter = new StreamWriter(fileStream))
            {
               fileStream.Seek(0, SeekOrigin.End);
               string output = string.Format("Process {0} writing timestamp {1}",
                  currentProcess.Id, DateTime.Now.ToLongTimeString());
               textWriter.WriteLine(output);
               Console.WriteLine(output);
            }
            Console.WriteLine("Process {0} releasing control",
               currentProcess.Id);
            mutex.ReleaseMutex();
            Thread.Sleep(1000);
         }
      }
   }
}
