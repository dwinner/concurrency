using System;
using System.Linq;
using static System.Console;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace SingleActionBlockLoadBalance
{
   internal static class Program
   {
      private static void Main()
      {
         var balancer = new LoadBalancerAsync<int, string>(new Func<int, string>[]
         {
            i => (i * 10).ToString(),
            i => (i * 100).ToString(),
            i => (i * 1000).ToString()
         }, 5);

         for (var i = 0; i < 10; i++)
            balancer.DoAsync(i)
               .ContinueWithMany(
                  ct => ct.ContinueWith(t => WriteLine(t.Result), OnlyOnRanToCompletion),
                  ct => ct.ContinueWith(t => WriteLine(t.Exception?.InnerExceptions.First()), OnlyOnFaulted)
               );

         ReadLine();
      }
   }
}