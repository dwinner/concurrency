using System;
using System.Threading;

namespace LockFreeViaSpinWait
{
   /// <summary>
   ///    Вспомогательные методы обновления в неблокирующем режиме
   /// </summary>
   public static class LockFreestyle
   {
      /// <summary>
      ///    Свободное от блокировок обновление
      /// </summary>
      /// <typeparam name="T">Параметр типа класса</typeparam>
      /// <param name="field">Целевой объект</param>
      /// <param name="updateFunctor">Функция обновления</param>
      /// <remarks>
      ///    Чем сложнее/дольше функция обновления, тем эффективнее данный тип блокировки,
      ///    в противном случае следует применять объекты мониторов
      /// </remarks>
      public static void LockFreeUpdate<T>(ref T field, Func<T, T> updateFunctor) where T : class
      {
         var spinWait = new SpinWait();
         while (true)
         {
            var snapshot1 = field;
            var updatedValue = updateFunctor(snapshot1);
            var snapshot2 = Interlocked.CompareExchange(ref field, updatedValue, snapshot1);
            if (snapshot1 == snapshot2)
            {
               return;
            }

            spinWait.SpinOnce();
         }
      }
   }
}