using System;

namespace PiplelineSample
{
   public static class ConsoleHelper
   {
      private static readonly object SyncOutput = new object();

      public static void WriteLine(string message)
      {
         lock (SyncOutput)
         {
            Console.WriteLine(message);
         }
      }

      public static void WriteLine(string message, string color)
      {
         lock (SyncOutput)
         {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            Console.WriteLine(message);
            Console.ResetColor();
         }
      }
   }
}