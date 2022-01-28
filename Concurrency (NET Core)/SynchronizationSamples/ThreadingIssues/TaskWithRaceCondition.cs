using System;

namespace ThreadingIssues
{
   public class TaskWithRaceCondition
   {
      public void RaceCondition(object o)
      {
         if (o is not StateObject state)
         {
            throw new ArgumentException("o must be a StateObject");
         }

         Console.WriteLine("starting RaceCondition - when does the issue occur?");

         var i = 0;
         while (true)
         {
            // lock (state) // no race condition with this lock
            {
               if (!state.ChangeState(i++))
               {
                  i = 0;
               }
            }
         }
      }
   }
}