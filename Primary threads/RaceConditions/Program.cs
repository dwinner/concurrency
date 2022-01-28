/**
 * Условия состязания
 */

using System;
using System.Threading.Tasks;

namespace RaceConditions
{
   class Program
   {
      static void Main()
      {
         RaceConditions();
         Console.ReadKey();
      }

      private static void RaceConditions()
      {
         var state = new StateObject();
         for (int i = 0; i < 2; i++)
         {
            Task.Run(() => new SampleTask().RaceCondition(state));
         }
      }
   }
}
