/**
 * Продвижение исключений по цепочке задач продолжения
 */

using System;
using System.Threading.Tasks;

namespace _08_PropagatingExceptions
{
   internal static class Program
   {
      private static void Main()
      {
         var gen1 = new Task(() => Console.WriteLine("First generation task"));

         Task gen2 = gen1.ContinueWith(task =>
         {
            Console.WriteLine("Second generation task - throws exception");
            throw new Exception();
         });

         Task gen3 = gen2.ContinueWith(task =>
         {
            if (task.Status == TaskStatus.Faulted && task.Exception != null)
               throw task.Exception.InnerException;

            Console.WriteLine("Third generation task");
         });

         gen1.Start();

         try
         {
            gen3.Wait();
         }
         catch (AggregateException aggregateException)
         {
            aggregateException.Handle(exception =>
            {
               Console.WriteLine("Handled exception of type: {0}", exception.GetType());
               return true;
            });
         }

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}