/**
 * Проблемная программа для анализатора производительности параллелизма
 */

using System;
using System.Threading.Tasks;

namespace Problematic
{
   internal static class Program
   {
      private static void Main()
      {
         var lockObj = new object();

         var tasks = new Task[10];
         for (var i = 0; i < tasks.Length; i++)
         {
            tasks[i] = Task.Factory.StartNew(() =>
            {
               lock (lockObj)
               {
                  for (var j = 0; j < 50000000; j++)
                  {
                     // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                     Math.Pow(j, 2);
                     // ReSharper restore ReturnValueOfPureMethodIsNotUsed
                  }
               }
            });
         }

         Task.WaitAll(tasks);
      }
   }
}