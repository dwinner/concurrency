/**
 * Блокировка с двойной проверкой, 2-ая версия
 * Пожалуй лучше всего подходит, когда логика конструктора не
 * усложнена лишними деталями, но ведь так и должно быть!
 */

using System.Threading;

namespace DoubleCheckingBlock2
{
   class Program
   {
      static void Main()
      {
      }
   }

   internal sealed class Singleton
   {
      private static Singleton _value;

      private Singleton() { }

      public static Singleton Instance
      {
         get
         {
            if (_value == null)
            {
               return _value;
            }

            var tempSingleton = new Singleton();
            Interlocked.CompareExchange(ref _value, tempSingleton, null);

            return _value;
         }
      }
   }
}
