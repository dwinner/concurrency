/**
 * Фабрики заданий
 */

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _10_TaskFactory
{
   class Program
   {
      static void Main()
      {
         var parentTask = new Task(() =>
         {
            var tokenSource = new CancellationTokenSource();
            var taskFactory = new TaskFactory<int>(
               tokenSource.Token,
               TaskCreationOptions.AttachedToParent,
               TaskContinuationOptions.ExecuteSynchronously,
               TaskScheduler.Default
            );

            // Задание создает и запускает 3 дочерних задания
            var childTasks = new[]
            {
               taskFactory.StartNew(() => Sum(tokenSource.Token, 10000), tokenSource.Token),
               taskFactory.StartNew(() => Sum(tokenSource.Token, 20000), tokenSource.Token),
               taskFactory.StartNew(() => Sum(tokenSource.Token, int.MaxValue), tokenSource.Token) // NOTE: OverflowException
            };

            // Если дочернее задание становится источником исключения,
            // отменяем все дочерние задания
            foreach (Task<int> aTask in childTasks)
            {
               aTask.ContinueWith(t => tokenSource.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
            }

            // После завершения дочерних заданий получаем максимальное
            // возвращенное значение и передаем его другому заданию для вывода
            taskFactory.ContinueWhenAll(
               childTasks,
               completedTasks => completedTasks.Where(t => !t.IsFaulted && !t.IsCanceled).Max(t => t.Result),
               CancellationToken.None
               ).ContinueWith(t => Console.WriteLine("The maximum is: {0}", t.Result),
                  TaskContinuationOptions.ExecuteSynchronously);
         });

         // После завершения дочерних заданий выводим,
         // в том числе, и необработанные исключения
         parentTask.ContinueWith(p =>
         {
            // Текст помещен в StringBuilder и однократно вызван
            // метод Console.WriteLine просто потому, что это задание
            // может выполняться параллельно с предыдущим,
            // и я не хочу путаницы в выводимом результате
            var stringBuilder =
               new StringBuilder(string.Format("The following exception(s) occured: {0}", Environment.NewLine));
            if (p.Exception != null)
            {
               foreach (var innerEx in p.Exception.Flatten().InnerExceptions)
               {
                  stringBuilder.AppendLine(" " + innerEx.GetType().ToString());
               }
            }
            Console.WriteLine(stringBuilder.ToString());
         }, TaskContinuationOptions.OnlyOnFaulted);

         // Запуск родительского задания, которое может запускать дочерние
         parentTask.Start();

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
