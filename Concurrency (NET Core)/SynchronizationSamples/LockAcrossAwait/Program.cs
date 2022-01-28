using System;
using System.Threading;
using System.Threading.Tasks;

namespace LockAcrossAwait
{
   internal class Program
   {
      private static readonly SemaphoreSlim AsyncLock = new(1);

      private static readonly AsyncSemaphore AsyncSemaphore = new();

      private static async Task Main()
      {
         await RunUseSemaphoreAsync();
         await RunUseAsyncSempahoreAsync();
         Console.ReadLine();
      }

      //private static object s_syncLock = new object();
      //static async Task IncorrectLockAsync()
      //{
      //    lock (s_syncLock)
      //    {
      //        Console.WriteLine($"{nameof(IncorrectLockAsync)} started");
      //        await Task.Delay(500);  // compiler error: cannot await in the body of a lock statement
      //        Console.WriteLine($"{nameof(IncorrectLockAsync)} ending");
      //    }
      //}


      private static async Task RunUseSemaphoreAsync()
      {
         Console.WriteLine(nameof(RunUseSemaphoreAsync));
         string[] messages = { "one", "two", "three", "four", "five", "six" };
         Task[] tasks = new Task[messages.Length];

         for (var i = 0; i < messages.Length; i++)
         {
            string message = messages[i];

            tasks[i] = Task.Run(async () => { await LockWithSemaphore(message); });
         }

         await Task.WhenAll(tasks);
         Console.WriteLine();
      }

      private static async Task RunUseAsyncSempahoreAsync()
      {
         Console.WriteLine(nameof(RunUseAsyncSempahoreAsync));
         string[] messages = { "one", "two", "three", "four", "five", "six" };
         Task[] tasks = new Task[messages.Length];

         for (var i = 0; i < messages.Length; i++)
         {
            string message = messages[i];

            tasks[i] = Task.Run(async () => { await UseAsyncSemaphore(message); });
         }

         await Task.WhenAll(tasks);
         Console.WriteLine();
      }

      private static async Task LockWithSemaphore(string title)
      {
         Console.WriteLine($"{title} waiting for lock");
         await AsyncLock.WaitAsync();
         try
         {
            Console.WriteLine($"{title} {nameof(LockWithSemaphore)} started");
            await Task.Delay(500);
            Console.WriteLine($"{title} {nameof(LockWithSemaphore)} ending");
         }
         finally
         {
            AsyncLock.Release();
         }
      }

      private static async Task UseAsyncSemaphore(string title)
      {
         using (await AsyncSemaphore.WaitAsync())
         {
            Console.WriteLine($"{title} {nameof(LockWithSemaphore)} started");
            await Task.Delay(500);
            Console.WriteLine($"{title} {nameof(LockWithSemaphore)} ending");
         }
      }
   }
}