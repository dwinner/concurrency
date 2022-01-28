/**
 * Передача объектов в задачу
 */

using System;
using System.Threading.Tasks;

namespace SettingTaskState
{
   internal class Program
   {
      private static void Main()
      {
         var firstTask = new Task(PrintMessage, "Hello, I am a state object");
         firstTask.Start();

         //var secondTask = new Task(Console.WriteLine, "Hello, I am a state object for second task");

         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }

      private static void PrintMessage(object message)
      {
         Console.WriteLine("Message: {0}", message);
      }
   }
}