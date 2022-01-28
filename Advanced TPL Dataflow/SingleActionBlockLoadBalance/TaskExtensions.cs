using System;
using System.Threading.Tasks;

namespace SingleActionBlockLoadBalance
{
   public static class TaskExtensions
   {
      public static Task[] ContinueWithMany<T>(this Task<T> @this,
         params Func<Task<T>, Task>[] continuations)
      {
         var results = new Task[continuations.Length];
         for (var i = 0; i < continuations.Length; i++)
            results[i] = continuations[i](@this);

         return results;
      }
   }
}