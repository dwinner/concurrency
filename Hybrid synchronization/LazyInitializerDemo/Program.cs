/**
 * Ленивое создание объекта в сценариях с ограниченной памятью
 */

using System;
using System.Threading;

namespace LazyInitializerDemo
{
   class Program
   {
      static void Main()
      {
         string name = null;
         // Так как имя равно null, запускается делегат и инициализирует поле имени
         LazyInitializer.EnsureInitialized(ref name, () => "Denny");
         Console.WriteLine(name);   // Denny

         // Так как имя отлично от null, делегат не запускается и имя не меняется
         LazyInitializer.EnsureInitialized(ref name, () => "Winner");
         Console.WriteLine(name);   // И снова Denny
      }
   }
}
