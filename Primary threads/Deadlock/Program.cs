/**
 * Клинч
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deadlock
{
   class Program
   {
      static void Main()
      {
         Deadlock();
         Console.ReadLine();
      }

      private static void Deadlock()
      {
         var s1 = new StateObject();
         var s2 = new StateObject();
         Task.Run(() => new SampleTask(s1, s2).Deadlock1());
         Task.Run(() => new SampleTask(s1, s2).Deadlock2());

         Thread.Sleep(100000);
      }
   }
}
