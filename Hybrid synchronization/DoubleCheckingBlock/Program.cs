/**
 * Блокировка с двойной проверкой, первая версия
 */

using System.Threading;

namespace DoubleCheckingBlock
{
   static class Program
   {
      static void Main()
      {
      }
   }

   internal sealed class Singleton
   {
      // Объект _lock требуется для обеспечения безопасности
      // в многопоточной среде. Наличие этого объекта предполагает,
      // что для создания одноэлементного объекта требуется больше
      // ресурсов, чем для объекта System.Object, и что эта процедура
      // может вовсе не понадобиться. В противном случае проще и
      // эффективнее получить одноэлементный объект в конструкторе класса
      private static readonly object Lock = new object();

      // Это поле ссылается на один объект Singleton
      private static /*volatile*/ Singleton _value;

      // Закрытый конструктор не дает внешнему коду создавать экземпляры
      private Singleton()
      {
         // Код инициализации объекта Singleton
      }

      // Открытый статический метод, возвращающий объект Singleton
      // (создавая его при необходимости)
      public static Singleton GetSingleton()
      {
         // Если объект уже создан, возвращаем его
         if (_value != null)
         {
            return _value;
         }

         Monitor.Enter(Lock);   // Если не создан, позволяем одному потоку сделать это
         if (_value == null)
         {
            // Если объекта все еще нет, создаем его
            var temp = new Singleton();

            // Сохраняем ссылку в переменной _value (публикуя ее через атомарность)
            Interlocked.Exchange(ref _value, temp);
         }
         Monitor.Exit(Lock);

         // Возвращаем ссылку на объект Singleton
         return _value;
      }
   }
}
