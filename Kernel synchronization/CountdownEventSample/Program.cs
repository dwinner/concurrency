/**
 * Сигнальное событие с обратным отсчетом
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEventSample
{
   class Program
   {
      static void Main()
      {
         const int taskCount = 4;
         var countdownEvent = new CountdownEvent(taskCount);
         var calculators = new Calculator[taskCount];

         for (int i = 0; i < taskCount; i++)
         {
            calculators[i] = new Calculator(countdownEvent);
            int localIndex = i;
            Task.Factory.StartNew(() => calculators[localIndex].Calculate(localIndex + 1, localIndex + 3));
         }

         countdownEvent.Wait();

         for (int i = 0; i < taskCount; i++)
         {
            Console.WriteLine("task for {0}, result: {1}", i, calculators[i].Result);
         }
      }
   }
}
