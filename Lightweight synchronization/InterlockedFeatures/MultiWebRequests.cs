using System;
using System.Net;
using System.Threading;

namespace InterlockedFeatures
{
   /// <summary>
   /// Неблокирующий мультизагрузчик веб-ресурсов
   /// </summary>
   internal sealed class MultiWebRequests
   {
      // Этот Helper-класс координирует все асинхронные операции
      private readonly AsyncCoordinator _coordinator = new AsyncCoordinator();

      // Набор web-серверов, к которым будут посылаться запросы
      private readonly WebRequest[] _requests =
      {
         WebRequest.Create("http://localhost"),
         WebRequest.Create("http://localhost")
      };

      // Создание массива ответов: по одному ответу на каждый запрос
      private readonly WebResponse[] _results = new WebResponse[2];

      public MultiWebRequests(int timeout = Timeout.Infinite)
      {
         // Одновременная асинхронная инициализация всех запросов
         for (int n = 0; n < _requests.Length; n++)
         {
            _coordinator.AboutToBegin();
            _requests[n].BeginGetResponse(EndGetResponse, n);
         }

         // Сообщаем классу Helper, что все операции инициализированы,
         // вызываем метод AllDone после завершения всех операций
         // или вызывается метод Cancel, или заканчивается время ожидания
         _coordinator.AllBegun(AllDone, timeout);
      }

      // Вызов этого метода показывает, что результат не имеет значения
      public void Cancel()
      {
         _coordinator.Cancel();
      }

      // Этот метод вызывается после получения ответа от всех серверов,
      // или вызывается метод Cancel, или заканчивается время ожидания
      private void AllDone(CoordinationStatus status)
      {
         switch (status)
         {
            case CoordinationStatus.Cancel:
               Console.WriteLine("The operation was cancelled");
               break;

            case CoordinationStatus.Timeout:
               Console.WriteLine("The operation timed-out");
               break;

            case CoordinationStatus.AllDone:
               Console.WriteLine("Here are the results from all the Web servers");
               for (int i = 0; i < _requests.Length; i++)
               {
                  Console.WriteLine("{0} returned {1} bytes.", _results[i].ResponseUri, _results[i].ContentLength);
               }
               break;
         }
      }

      // Этот метод вызывается при ответе каждого сервера
      private void EndGetResponse(IAsyncResult asyncResult)
      {
         // Получение индекса запроса
         var requestIndex = (int)asyncResult.AsyncState;

         // Сохраняем ответ под тем же самым индексом
         _results[requestIndex] = _requests[requestIndex].EndGetResponse(asyncResult);

         // Сообщаем классу Helper, что сервер ответил
         _coordinator.JustEnded();
      }
   }
}