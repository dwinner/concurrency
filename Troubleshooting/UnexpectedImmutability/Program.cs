/**
 * Ложная неизменяемость
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnexpectedImmutability
{
   internal static class Program
   {
      private static void Main()
      {
         var immutable = new MyImmutableType();

         var tokenSource = new CancellationTokenSource();

         var task1 = new Task(() =>
         {
            while (true)
            {
               double circ = 2 * immutable.RefData.Pi * MyImmutableType.CircleSize;
               Console.WriteLine("Circumference: {0}", circ);
               if (Math.Abs(circ - 4) < Double.Epsilon)
               {
                  Console.WriteLine("Mutation detected");
                  break;
               }

               tokenSource.Token.WaitHandle.WaitOne(250);
            }
         }, tokenSource.Token);

         task1.Start();

         Thread.Sleep(1000);

         immutable.RefData.Pi = 2;

         task1.Wait(tokenSource.Token);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class MyReferenceData
   {
      public double Pi = 3.14;
   }

   internal class MyImmutableType
   {
      public const int CircleSize = 1;
      public readonly MyReferenceData RefData = new MyReferenceData();
   }
}