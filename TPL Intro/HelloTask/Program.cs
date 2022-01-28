/**
 * Простая задача
 */

using System;
using System.Threading.Tasks;

namespace HelloTask
{
   internal class Program
   {
      private static void Main()
      {
         Task.Factory.StartNew(() => Console.WriteLine("Hello World"));
         // Ждем ввода до завершения программы
         Console.WriteLine("Main method complete. Press enter to finish.");
         Console.ReadLine();
      }
   }
}