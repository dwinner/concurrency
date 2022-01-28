using System;
using System.Threading;
using System.Windows.Forms;

namespace SemaphoreDemo
{
   public partial class MainForm : Form
   {
      // Семафор одновременно открывает два входа (при 2-х возможных).
      // При запуске программы некоторые потоки могут быть заблокированы
      // в случае необходимости
      private readonly Semaphore _semaphore = new Semaphore(2, 2);
      private readonly ProgressBar[] _progressBars = new ProgressBar[3];
      private readonly Thread[] _threads = new Thread[3];

      private const int MAX_VALUE = 1000000;

      public MainForm()
      {
         InitializeComponent();

         _progressBars[0] = firstProgressBar;
         _progressBars[1] = secondProgressBar;
         _progressBars[2] = thirdProgressBar;

         for (int i = 0; i < 3; i++)
         {
            _progressBars[i].Minimum = 0;
            _progressBars[i].Maximum = MAX_VALUE;
            _progressBars[i].Style = ProgressBarStyle.Continuous;
         }
      }

      private void buttonStart_Click(object sender, EventArgs e)
      {
         buttonStart.Enabled = false;
         for (int i = 0; i < 3; i++)
         {
            _threads[i] = new Thread(IncrementThread) { IsBackground = true };
            _threads[i].Start(i);
         }
      }

      private void IncrementThread(object state)
      {
         int threadNumber = (int)state;
         int value = 0;
         while (value < MAX_VALUE)
         {
            // Только двум потокам будет разрешено
            // войти в этот раздел
            _semaphore.WaitOne();
            for (int i = 0; i < 100000; i++)
            {
               ++value;
               UpdateProgress(threadNumber, value);
            }
            _semaphore.Release();
         }
      }

      private void UpdateProgress(int thread, int value)
      {
         if (value <= MAX_VALUE)
         {
            // Нужен метод Invoke, потому что обновление
            // происходит в потоке пользовательского интерфейса
            _progressBars[thread].Invoke(
               new MethodInvoker(() => _progressBars[thread].Value = value));
         }
      }

      #region Interlocked

      /*
            private void ThreadProc()
            {
               int value = 13;

               Interlocked.Increment(ref value);   // добавляет один
               Interlocked.Decrement(ref value);   // вычитает один
               Interlocked.Add(ref value, 13);     // добавляет 13
               int originalValue = Interlocked.Exchange(ref value, 99); // устанавливает значение в 99 и возвращает оригинальное
               string s1 = "Hello";
               string sNew = "Bonjour";
               string sCompare = "Hello";
               // Если s1 = sCompare, то s1 = sNew
               string sOriginal = Interlocked.CompareExchange<string>(ref s1, sNew, sCompare);
            }
      */

      #endregion

   }
}
