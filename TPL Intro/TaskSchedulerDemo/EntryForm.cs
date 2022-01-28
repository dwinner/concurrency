/**
 * Планировщик заданий контекста синхронизации
 * NOTE: Другие планировщики доступны по адресу: http://code.msdn.microsoft.com/ParExtSamples
 */

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _11_TaskSchedulerDemo
{
   internal sealed class EntryForm : Form
   {
      //private readonly TaskScheduler _syncContexTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      private CancellationTokenSource _cancellationToken;

      public EntryForm()
      {
         Text = "Synchronization context task scheduler demo";
         Visible = true;
         Width = 400;
         Height = 100;
      }

      protected override void OnMouseClick(MouseEventArgs e)
      {
         if (_cancellationToken != null)  // Операция начата, отменяем ее
         {
            _cancellationToken.Cancel();
            _cancellationToken = null;
         }
         else   // Операция не начата, начинаем ее
         {
            Text = "Operation running";
            _cancellationToken = new CancellationTokenSource();

            // Это задание использует заданный по умолчанию планировщик
            // и выполняет поток из пула
            var task = new Task<int>(() => Sum(_cancellationToken.Token, 20000), _cancellationToken.Token);
            task.Start();

            // Эти задания используют планировщик контекста синхронизации
            // и выполняют поток графического интерфейса
            task.ContinueWith(aTask => Text = string.Format("Result: {0}", aTask.Result),
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnRanToCompletion,
               TaskScheduler.FromCurrentSynchronizationContext());

            task.ContinueWith(aTask => Text = "Operation canceled",
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnCanceled,
               TaskScheduler.FromCurrentSynchronizationContext());

            task.ContinueWith(aTask => Text = "Operation faulted",
               CancellationToken.None,
               TaskContinuationOptions.OnlyOnFaulted,
               TaskScheduler.FromCurrentSynchronizationContext());
         }
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
