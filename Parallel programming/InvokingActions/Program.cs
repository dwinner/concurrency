/**
 * Параллельный вызов действий
 */

using System;
using System.Threading.Tasks;

namespace _02_InvokingActions
{
   internal static class Program
   {
      private static void Main()
      {
         // Вызов действий через лямбда-выражения
         Parallel.Invoke(() => Console.WriteLine("Action 1"),
            () => Console.WriteLine("Action 2"),
            () => Console.WriteLine("Action 3"));

         // Явное создание массива действий
         var actions = new Action[3];
         actions[0] = () => Console.WriteLine("Action 4");
         actions[1] = () => Console.WriteLine("Action 5");
         actions[2] = () => Console.WriteLine("Action 6");

         // Вызов массива действий
         Parallel.Invoke(actions);

         // Создаем такой же эффект, используя задачи
         Task parentTask = Task.Factory.StartNew(() =>
         {
            foreach (Action action in actions)
            {
               Task.Factory.StartNew(action, TaskCreationOptions.AttachedToParent);
            }
         });

         parentTask.Wait();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}