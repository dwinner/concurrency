using System;
using System.Diagnostics;

namespace LockFreeViaSpinWait
{
   public sealed class AutoStopwatch : Stopwatch, IDisposable
   {
      public AutoStopwatch()
      {
         Start();
      }

      public void Dispose()
      {
         Stop();
         OnTotalElapsed(new ElapsedEventArgs(Elapsed));
      }

      public event EventHandler<ElapsedEventArgs> TotalElapsed;

      private void OnTotalElapsed(ElapsedEventArgs e) => TotalElapsed?.Invoke(this, e);
   }
}