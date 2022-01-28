/**
 * Легковесная блокировка
 */

using System;
using System.Threading.Tasks;

namespace LightweightSynchronization
{
   class Program
   {
      static void Main()
      {
         const int numTasks = 20;
         var state = new SharedState();
         var tasks = new Task[numTasks];

         for (int i = 0; i < numTasks; i++)
         {
            tasks[i] = Task.Run(() => new Job(state).DoTheJob());
         }

         for (int i = 0; i < numTasks; i++)
         {
            tasks[i].Wait();
         }

         Console.WriteLine("summarized {0}", state.State);  // 1000000
      }
   }
}
