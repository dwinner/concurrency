namespace Deadlock
{
   public class StateObject
   {
      private int _state;

      public void ChangeState(int loop)
      {
         if (_state == 5)
         {
            _state++;
         }
         _state = 5;
      }
   }
}