using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace BackgroundWorkerSample
{
   /// <summary>
   /// Компонент BackgroundWorker как способ организации неблокирующих вычислений
   /// </summary>
   public partial class MainWindow
   {
      private BackgroundWorker _backgroundWorker;

      public MainWindow()
      {
         InitializeComponent();
         SetupBackgroundWorker();
      }

      private void SetupBackgroundWorker()
      {
         _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
         _backgroundWorker.DoWork += OnDoWork;
         _backgroundWorker.RunWorkerCompleted += OnWorkerCompleted;
         _backgroundWorker.ProgressChanged += OnProgressChanged;
      }

      private void OnDoWork(object sender, DoWorkEventArgs e)
      {
         var inputArg = e.Argument as Tuple<int, int>;
         if (inputArg == null)
         {
            return;
         }

         for (int i = 0; i < 10; i++)
         {
            Thread.Sleep(500);
            _backgroundWorker.ReportProgress(i * 10);
            if (_backgroundWorker.CancellationPending)
            {
               e.Cancel = true;
               return;
            }
         }

         e.Result = inputArg.Item1 + inputArg.Item2;
      }

      private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         ResultProgressBar.Value = e.ProgressPercentage;
      }

      private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         ResulTextBox.Text = e.Cancelled ? "Canceled" : e.Result.ToString();
         CalculateButton.IsEnabled = true;
         CancelButton.IsEnabled = !CalculateButton.IsEnabled;
         ResultProgressBar.Value = 100;
      }

      private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
      {
         int x, y;
         if (int.TryParse(XTextBox.Text, out x) && int.TryParse(YTextBox.Text, out y))
         {
            CalculateButton.IsEnabled = false;
            CancelButton.IsEnabled = !CalculateButton.IsEnabled;
            ResulTextBox.Text = string.Empty;
            ResultProgressBar.Value = 0;
            _backgroundWorker.RunWorkerAsync(Tuple.Create(x, y));
         }
         else
         {
            MessageBox.Show("Input error");
         }
      }

      private void CancelButton_OnClick(object sender, RoutedEventArgs e)
      {
         _backgroundWorker.CancelAsync();
      }
   }
}
