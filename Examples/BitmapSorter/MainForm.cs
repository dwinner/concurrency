using BitmapSorter.Properties;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ThreadingTimer = System.Threading.Timer;

namespace BitmapSorter
{
   public partial class MainForm : Form
   {
      private ScrambledBitmap _scrambledBitmap;
      private Bitmap _displayBitmap;
      private ThreadingTimer _updateTimer;

      public MainForm()
      {
         InitializeComponent();
         OpenBitmap("Elements_Small.png");
      }

      private void OnDragOver(object sender, DragEventArgs e)
      {
         e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
            ? DragDropEffects.Copy
            : DragDropEffects.None;
      }

      private void OnDragDrop(object sender, DragEventArgs e)
      {
         if (e.Data.GetDataPresent(DataFormats.FileDrop))
         {
            OpenBitmap(e.Data.GetData(DataFormats.FileDrop) as string);
         }
      }

      private void OpenBitmap(string filename)
      {
         Bitmap bitmap = new Bitmap(filename);
         _scrambledBitmap = new ScrambledBitmap(bitmap);
         SetDisplayBitmap(bitmap);
      }

      private void buttonScrambleSort_Click(object sender, EventArgs e)
      {
         if (_scrambledBitmap.IsSorted)
         {
            _scrambledBitmap.Scramble();
            buttonScrambleSort.Text = Resources.MainForm_buttonScrambleSort_Click__Sort;
            SetDisplayBitmap(_scrambledBitmap.ToBitmap());
            _updateTimer = new ThreadingTimer(stateObj =>
            {
               SetDisplayBitmap(_scrambledBitmap.ToBitmap());
               if (!_scrambledBitmap.IsSorted)
                  return;
               _updateTimer.Dispose();
               Invoke(new MethodInvoker(() =>   // Note: Обновляем UI в его акторе
               {
                  buttonScrambleSort.Enabled = true;
               }));
            }, null, 1000, 1000);
         }
         else
         {
            buttonScrambleSort.Enabled = false;
            buttonScrambleSort.Text = Resources.MainForm_buttonScrambleSort_Click__Scramble;
            // Note: Используем пул потоков CLR для нашей сортировки
            ThreadPool.QueueUserWorkItem(state => _scrambledBitmap.Sort());
         }
      }

      private void SetDisplayBitmap(Bitmap newBitmap)
      {
         if (_displayBitmap != null)
         {
            _displayBitmap.Dispose();
         }
         _displayBitmap = newBitmap;
         pictureBox.Image = _displayBitmap;
      }
   }
}
