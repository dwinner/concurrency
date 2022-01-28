/**
 * Синхронизация в пользовательском режиме (через процессорные инструкции)
 */

using System;

namespace InterlockedFeatures
{
   internal static class Program
   {
      static void Main()
      {
         var webRequests = new MultiWebRequests(500);
         Console.WriteLine("Press any key to continue...");
         Console.ReadKey();
      }
   }
}
