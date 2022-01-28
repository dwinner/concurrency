using System;

namespace LockFreeViaSpinWait
{
   public sealed class ElapsedEventArgs : EventArgs
   {
      public ElapsedEventArgs(TimeSpan spent)
      {
         Spent = spent;
      }

      public TimeSpan Spent { get; }
   }
}