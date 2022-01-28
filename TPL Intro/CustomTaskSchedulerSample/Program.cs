/**
 * Реализация собственного планировщика задач
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _20_CustomTaskSchedulerSample
{
   internal static class Program
   {
      private static void Main()
      {
         var scheduler = new CustomScheduler();

         Console.WriteLine("Custom scheduler Id: {0}", scheduler.Id);
         Console.WriteLine("Default scheduler Id: {0}", TaskScheduler.Default.Id);

         var tokenSource = new CancellationTokenSource();

         var aTask = new Task(() =>
         {
            Console.WriteLine("Task {0} executed by scheduler {1}", Task.CurrentId, TaskScheduler.Current.Id);

            // Создаем дочернюю задачу, которая будет использовать тот же планировщик, что и родитель
            Task.Factory.StartNew(
               () => Console.WriteLine("Task {0} executed by scheduler {1}", Task.CurrentId, TaskScheduler.Current.Id),
               tokenSource.Token);

            // А эта дочерняя задача будет использовать планировщик по умолчанию
            Task.Factory.StartNew(
               () => Console.WriteLine("Task {0} executed by scheduler {1}", Task.CurrentId, TaskScheduler.Current.Id),
               tokenSource.Token, TaskCreationOptions.None, TaskScheduler.Default);
         });

         aTask.Start(scheduler);

         // Задача продолжения, использующая планировщик по умолчанию
         aTask.ContinueWith(
            task => Console.WriteLine("Task {0} executed by scheduler {1}", Task.CurrentId, TaskScheduler.Current.Id),
            tokenSource.Token);

         // Задача продолжения, использующая наш планировщик
         aTask.ContinueWith(
            task => Console.WriteLine("Task {0} executed by scheduler {1}", Task.CurrentId, TaskScheduler.Current.Id),
            scheduler);
      }
   }
}