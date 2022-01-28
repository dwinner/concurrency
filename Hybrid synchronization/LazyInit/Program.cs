/**
 * Отложенное создание как шаблон программирования
 */

using System;
using System.Threading;

namespace LazyInit
{
   static class Program
   {
      static void Main()
      {
         // Создание оболочки отложенной инициализации для получения DateTime
         var s = new Lazy<string>(() => DateTime.Now.ToLongTimeString(), LazyThreadSafetyMode.PublicationOnly);

         Console.WriteLine(s.IsValueCreated);   // Возвращается false, так как
         // запроса к Value еще не было
         Console.WriteLine(s.Value);            // Вызывается делегат инициализации
         Console.WriteLine(s.IsValueCreated);   // Возвращается true, так как
         // был запрос к Value
         Thread.Sleep(10000);                   // Ждем 10 секунд и снова выводим время
         Console.WriteLine(s.Value);            // Теперь делегат не вызывается, результат прежний
      }
   }
}
