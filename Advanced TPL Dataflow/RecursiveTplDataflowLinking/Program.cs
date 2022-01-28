using System;

namespace RecursiveTplDataflowLinking
{
   internal static class Program
   {
      private static void Main()
      {
         var walker = new DirectoryWalker(Console.WriteLine);
         walker.WalkAsync(@"d:\Cartoons").Wait();
      }
   }
}