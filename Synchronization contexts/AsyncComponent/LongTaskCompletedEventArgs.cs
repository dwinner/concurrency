using System;
using System.ComponentModel;

namespace Wrox.ProCSharp.Threading
{
   public class LongTaskCompletedEventArgs : AsyncCompletedEventArgs
   {
      public LongTaskCompletedEventArgs(string output, Exception e, bool cancelled, object state)
         : base(e, cancelled, state)
      {
         _output = output;
      }

      private readonly string _output;

      public string Output
      {
         get
         {
            RaiseExceptionIfNecessary();
            return _output;
         }
      }
   }
}