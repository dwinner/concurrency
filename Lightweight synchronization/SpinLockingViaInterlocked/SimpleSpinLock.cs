using System.Threading;

namespace SpinLockingViaInterlocked
{
   internal struct SimpleSpinLock
   {
      private int _resourceInUse;   // 0=false (по умолчанию), 1=true

      public void Enter()
      {
         // Указываем, что ресурс используется, и если этот поток
         // переводит его из свободного состояния, возвращаем управление
         while (Interlocked.Exchange(ref _resourceInUse, 1) != 0)
         {
            /* Здесь что-то происходит */
         }
      }

      public void Leave()
      {
         // Помечаем ресурс как свободный
         Thread.VolatileWrite(ref _resourceInUse, 0);
      }
   }
}