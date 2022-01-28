/**
 * Межпроцесссорный Mutex
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _11_InterprocessMutex
{
   internal static class Program
   {
      private static void Main()
      {
         const string mutexName = "NamedMutex";
         Mutex namedMutex;

         try
         {
            namedMutex = Mutex.OpenExisting(mutexName);
         }
         catch (WaitHandleCannotBeOpenedException)
         {
            namedMutex = new Mutex(false, mutexName);
         }

         var task = new Task(() =>
         {
            while (true)
            {
               Console.WriteLine("Waiting to acquire Mutex");
               namedMutex.WaitOne();
               Console.WriteLine("Acquired Mutex - press enter to release");
               Console.ReadLine();
               namedMutex.ReleaseMutex();
               Console.WriteLine("Released Mutex");
            }
         });

         task.Start();
         task.Wait();
      }
   }
}