/**
 * Декрементальное событие синхронизации
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _14_CountDownEventSample
{
   internal static class Program
   {
      private static void Main()
      {
         var countdownEvent = new CountdownEvent(5);

         var rnd = new Random();

         var tasks = new Task[countdownEvent.InitialCount + 1];

         for (int i = 0; i < tasks.Length; i++)
         {
            tasks[i] = new Task(() =>
            {
               Thread.Sleep(rnd.Next(500, 1000));
               Console.WriteLine("Task {0} signalling event", Task.CurrentId);
               countdownEvent.Signal();
            });
         }

         // Задача, ждущая событие на CountDownEvent
         tasks[tasks.Length - 1] = new Task(() =>
         {
            Console.WriteLine("Rendezvous task waiting");
            countdownEvent.Wait();
            Console.WriteLine("Event has been set");
         });

         foreach (Task task in tasks)
         {
            task.Start();
         }

         Task.WaitAll(tasks);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}