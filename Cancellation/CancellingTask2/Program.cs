/**
 * Отмена заданий
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace _07_CancellingTask
{
   class Program
   {
      static void Main()
      {
         var cancellationTokenSource = new CancellationTokenSource();
         var task = new Task<int>(() => Sum(cancellationTokenSource.Token, 10000), cancellationTokenSource.Token);
         task.Start();
         // Позднее отменим CancellationTokenSource, чтобы отменить Task
         cancellationTokenSource.Cancel();   // NOTE: Это асинхронный запрос, задача уже может быть завершена

         try
         {
            // В случае отмены задания метод Result генерирует
            // исключение AggregateException
            Console.WriteLine("The sum is: {0}", task.Result); // Значение Int32
         }
         catch (AggregateException aggregateEx)
         {
            // Считаем обработанными все объекты OperationCanceledException
            // Все остальные исключения попадают в новый объект AggregateException,
            // состоящий только из необработанных исключений
            aggregateEx.Handle(e => e is OperationCanceledException);

            // Строка выполняется, если все исключения уже обработаны
            Console.WriteLine("Sum was canceled");
         }

         Console.ReadKey();
      }

      private static int Sum(CancellationToken cancellationToken, int n)
      {
         int sum = 0;
         for (; n > 0; n--)
         {
            // Следующая строка приводит к исключению OperationCanceledException
            // при вызове метода Cancel для объекта CancellationTokenSource,
            // на который ссылкается маркер
            cancellationToken.ThrowIfCancellationRequested();

            checked { sum += n; }   // При больших n появляется System.OverflowException
         }
         return sum;
      }
   }
}
