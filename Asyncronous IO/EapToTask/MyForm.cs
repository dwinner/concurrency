using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EapToTask
{
   public partial class MyForm : Form
   {
      private const string LocalHost = "http://localhost";

      public MyForm()
      {
         InitializeComponent();
      }

      protected override void OnClick(EventArgs e)
      {
         // Класс System.Net.WebClient поддерживает EAP
         var webClient = new WebClient();

         // Создание объектов TaskCompletionSource и Task
         var taskCompletionSource = new TaskCompletionSource<string>();

         // Когда завершается загрузка строки, объект WebClient активизирует
         // событие DownloadStringCompleted, вызывающее метод обратного вызова
         webClient.DownloadStringCompleted += (sender, args) =>
         {
            // Этот код всегда выполняется GUI-потоком: задает состояние задания
            if (args.Cancelled)
               taskCompletionSource.SetCanceled();
            else if (args.Error != null)
               taskCompletionSource.SetException(args.Error);
            else
               taskCompletionSource.SetResult(args.Result);
         };

         // Продолжает задание объект Task, выводящий результат в окне
         // Note: флаг ExecuteSynchronously обеспечивает выполнение кода
         // GUI-потоком; без него код будет выпоняться потоком из пула
         taskCompletionSource.Task.ContinueWith(task =>
         {
            try
            {
               MessageBox.Show(task.Result);
            }
            catch (AggregateException aggregateException)
            {
               MessageBox.Show(aggregateException.GetBaseException().Message);
            }
         }, TaskContinuationOptions.ExecuteSynchronously);

         // Начало асинхронной операции
         webClient.DownloadStringAsync(new Uri(LocalHost));

         base.OnClick(e);
      }
   }
}
