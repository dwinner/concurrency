using System;
using System.Runtime.Serialization;

namespace SingleActionBlockLoadBalance
{
   [Serializable]
   public class TooBusyException : Exception
   {
      public TooBusyException()
      {
      }

      public TooBusyException(string message) : base(message)
      {
      }

      public TooBusyException(string message, Exception inner) : base(message, inner)
      {
      }

      protected TooBusyException(
         SerializationInfo info,
         StreamingContext context) : base(info, context)
      {
      }
   }
}