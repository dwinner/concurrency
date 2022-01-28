using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TextTokenizer
{
   public partial class Mainform : Form
   {
      private byte[] _buffer;

      public Mainform()
      {
         InitializeComponent();
      }

      private void buttonGo_Click(object sender, EventArgs e)
      {
         UpdateProgress("Reading file");
         FileStream inputStream = new FileStream(textBoxUrl.Text, FileMode.Open);
         _buffer = new byte[inputStream.Length];

         // Передать поток inputStream в качестве аргумента методу XXXDone         
         inputStream.BeginRead(_buffer, 0, _buffer.Length, FileReadDone, inputStream);

         // Объект IAsyncResult можно использовать для отслеживания
         // хода выполнения метода
         // Пока происходит чтение файла, мы можем щелкать по кнопкам
         // и даже завершить программу
      }

      private void FileReadDone(IAsyncResult result)
      {
         UpdateProgress("File read done");
         FileStream inputStream = result.AsyncState as FileStream;
         if (inputStream != null)
            inputStream.Close();

         // Начинаем асинхронный подсчет слов

         TokenCounter counter = new TokenCounter(Encoding.ASCII.GetString(_buffer));
         counter.BeginCount(CountDone, counter);
         UpdateProgress("Counting tokens");

         // Если мы хотим ждать завершения операции, нужно вызвать : counter.EndCount(counterResult);
      }

      private void CountDone(IAsyncResult result)
      {
         UpdateProgress("Count done");
         TokenCounter counter = result.AsyncState as TokenCounter;

         listViewTokens.Invoke(new MethodInvoker(() =>
         {
            listViewTokens.BeginUpdate();
            if (counter != null)
            {
               foreach (WordCount count in counter.WordCounts)
               {
                  ListViewItem item = new ListViewItem(count.Word);
                  item.SubItems.Add(count.Count.ToString("N0"));
                  listViewTokens.Items.Add(item);
               }
            }
            listViewTokens.EndUpdate();
         }));
      }

      private void UpdateProgress(string message)
      {
         BeginInvoke(
            new MethodInvoker(() => labelStatus.Text = message));
      }
   }
}
