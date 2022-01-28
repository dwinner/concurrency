/**
 * Неправильная передача локальных переменных
 */

using System;
using System.Threading.Tasks;

namespace LocalVariableEvaluation
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем изапускаем "плохие" задачи
         for (int i = 0; i < 5; i++)
         {
            Task.Factory.StartNew(() => { Console.WriteLine("Task {0} has counter value: {1}", Task.CurrentId, i); });
         }

         Console.ReadLine();

         // Создаем и запускаем "хорошие" задачи
         for (int i = 0; i < 5; i++)
         {
            int localI = i;
            Task.Factory.StartNew(() => Console.WriteLine("Task {0} has counter value: {1}", Task.CurrentId, localI));
         }

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}