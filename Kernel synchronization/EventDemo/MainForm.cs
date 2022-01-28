using System;
using System.Threading;
using System.Windows.Forms;

namespace EventDemo
{
   public partial class MainForm : Form
   {
      private readonly ManualResetEvent _manualEvent = new ManualResetEvent(false);
      private readonly AutoResetEvent _autoEvent = new AutoResetEvent(false);

      private readonly ProgressBar[] _progressBars = new ProgressBar[3];
      private readonly Thread[] _threads = new Thread[3];

      private const int MAX_VALUE = 1000000;

      private bool _manual = true;

      public MainForm()
      {
         InitializeComponent();
         ReInit();
      }

      private void ReInit()
      {
         _manualEvent.Reset();
         _autoEvent.Reset();

         _progressBars[0] = firstProgressBar;
         _progressBars[1] = secondProgressBar;
         _progressBars[2] = thirdProgressBar;

         for (int i = 0; i < 3; i++)
         {
            _progressBars[i].Minimum = 0;
            _progressBars[i].Maximum = MAX_VALUE;
            _progressBars[i].Style = ProgressBarStyle.Continuous;
            _progressBars[i].Value = 0;
         }

         for (int i = 0; i < 3; i++)
         {
            if (_threads[i] != null)
               _threads[i].Abort();
            _threads[i] = new Thread(ThreadProc) { IsBackground = true };
            _threads[i].Start(i);
         }
      }

      private void buttonSet_Click(object sender, EventArgs e)
      {
         bool success = _manual ? _manualEvent.Set() : _autoEvent.Set();
         if (!success)
            throw new ApplicationException("Something wrong");
      }

      private void buttonReset_Click(object sender, EventArgs e)
      {
         bool success = _manual ? _manualEvent.Reset() : _autoEvent.Reset();
         if (!success)
            throw new ApplicationException("Something wrong");
      }

      private void ThreadProc(object state)
      {
         int threadNumber = (int)state;
         int value = 0;
         while (value < MAX_VALUE)
         {
            bool success = _manual ? _manualEvent.WaitOne() : _autoEvent.WaitOne();
            if (!success)
               throw new ApplicationException("Something wrong");
            for (int i = 0; i < 100000; ++i)
            {
               ++value;
               UpdateProgress(threadNumber, value);
               Thread.Sleep(0);  // Note: таким образом мы не загружаем процессор на 100% (равнозначно Thread.Yield())
            }
         }
      }

      private void UpdateProgress(int thread, int value)
      {
         // Нужен метод Invoke, потому что обновление происходит в UI-потоке
         _progressBars[thread].Invoke(
            new MethodInvoker(() => _progressBars[thread].Value = value));
      }

      private void OnEventTypeChanged(object sender, EventArgs e)
      {
         _manual = radioButtonManual.Checked;
         buttonReset.Enabled = _manual;
         ReInit();
      }
   }
}
