using System;

namespace TPLPrimes.Extensions
{
   public static class IntExtensions
   {
      public static bool IsPrime(this uint number)
      {
         // Проверка на четность

         if (number % 2 == 0)
            return number == 2;

         // После извлечения квадратного корня проверка не нужна

         uint max = (uint)Math.Sqrt(number);
         for (uint i = 3; i <= max; i += 2)
            if ((number % i) == 0)
               return false;
         return true;
      }
   }
}