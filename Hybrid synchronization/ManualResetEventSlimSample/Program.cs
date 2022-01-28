/**
 * Сигнальные события
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimSample
{
   class Program
   {
      private const int TaskCount = 4;

      static void Main()
      {
         var events = new ManualResetEventSlim[TaskCount];
         var waitHandles = new WaitHandle[TaskCount];
         var calculators = new Calculator[TaskCount];

         for (int i = 0; i < TaskCount; i++)
         {
            int localIndex = i;
            events[i] = new ManualResetEventSlim(false);
            waitHandles[i] = events[i].WaitHandle;
            calculators[i] = new Calculator(events[i]);
            Task.Run(() => calculators[localIndex].Calculation(localIndex + 1, localIndex + 3));
         }

         for (int i = 0; i < TaskCount; i++)
         {
            int index = WaitHandle.WaitAny(waitHandles);
            if (index == WaitHandle.WaitTimeout)
            {
               Console.WriteLine("Timeout");
            }
            else
            {
               events[index].Reset();
               Console.WriteLine("finished task for {0}, result: {1}", index, calculators[index].Result);
            }
         }
      }
   }
}
