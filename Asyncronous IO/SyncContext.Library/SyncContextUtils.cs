using System;
using System.Threading;

namespace SyncContext.Library
{
   /// <summary>
   /// Вспомогательный класс, который помогает вызывать методы в текущих контекстах синхронизации
   /// </summary>
   public static class SyncContextUtils
   {
      /// <summary>
      /// Обертывает вызов метода в текущий контекст синхронизации
      /// </summary>
      /// <param name="callback">Ссылка на метод, который должен быть вызван при завершении асинхронной операции</param>
      /// <returns>Ссылка на метод в текущем контексте синхронизации</returns>
      public static AsyncCallback SyncContextCallback(AsyncCallback callback)
      {
         // Фиксируем производный от SynchronizationContext объект вызывающего потока
         var syncContext = SynchronizationContext.Current;

         // При отсутствии контекста синхронизации возвращаем переданное в метод
         if (syncContext == null)
            return callback;

         // Возвращаем делегат, который отправляет в фиксированный контекст синхронизации
         // метод, передающий в исходный вызов AsyncCallback аргумент IAsyncResult
         return asyncResult => syncContext.Post(result => callback((IAsyncResult)result), asyncResult);
      }
   }
}
