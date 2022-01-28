/**
 * Отладка параллельных приложений
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exercising
{
   internal static class Program
   {
      private static CountdownEvent _countdownEvent;
      private static SemaphoreSlim _slimA, _slimB;

      private static void Main()
      {
         _slimA = new SemaphoreSlim(2);
         _slimB = new SemaphoreSlim(2);

         const int taskCount = 10;

         _countdownEvent = new CountdownEvent(taskCount);

         var tasks = new Task[10];
         for (var i = 0; i < taskCount; i++)
         {
            tasks[i] = Task.Factory.StartNew(o => InitialMethod((int)o), i);
         }

         _countdownEvent.Wait();
         Console.WriteLine();
      }

      private static void InitialMethod(int state)
      {
         if (state % 2 == 0)
         {
            MethodA(state);
         }
         else
         {
            MethodB(state);
         }
      }

      private static void MethodB(int arg)
      {
         if (arg < 5)
         {
            TerminalMethodA();
         }
         else
         {
            TerminalMethodB();
         }
      }

      private static void TerminalMethodB()
      {
         _countdownEvent.Signal();
         _slimB.Wait();
         for (var i = 0; i < 500000000; i++)
         {
            Math.Pow(i, 3);
         }
         _slimB.Release();
      }

      private static void TerminalMethodA()
      {
         _countdownEvent.Signal();
         _slimA.Wait();
         for (var i = 0; i < 500000000; i++)
         {
            Math.Pow(i, 2);
         }
         _slimA.Release();
      }

      private static void MethodA(int arg)
      {
         if (arg < 5)
         {
            TerminalMethodA();
         }
         else
         {
            TerminalMethodB();
         }
      }
   }
}