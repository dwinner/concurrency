/**
 * Избегаем deadlock'ов, удаляя плохих участников барьера
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _12_ReducingParticipations
{
   internal static class Program
   {
      private static void Main()
      {
         var barrier = new Barrier(2);

         // Создаем "хорошую" задачу
         Task.Factory.StartNew(() =>
         {
            Console.WriteLine("Good task starting phase 0");
            barrier.SignalAndWait();
            Console.WriteLine("Good task starting phase 1");
            barrier.SignalAndWait();
            Console.WriteLine("Good task completed");
         });

         /* Создаем задачу, которая выбрасывает исключение
          с выборорочной задачей продолжения, которая удаляет
          "плохого" участника барьера */
         Task.Factory.StartNew(() =>
         {
            Console.WriteLine("Bad task 1 throwing exception");
            throw new Exception();
         }).ContinueWith(antecedentTask =>
         {
            Console.WriteLine("Reducing the barrier participant count");
            barrier.RemoveParticipant();  // NOTE: Без этого была бы заблокирована "хорошая" задача
         }, TaskContinuationOptions.OnlyOnFaulted);

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}