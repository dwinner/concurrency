using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLINQDataProcessingWithCancellation
{
   public partial class MainForm : Form
   {
      private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

      private void btnCancel_Click(object sender, EventArgs e)
      {
         _cancelToken.Cancel();
      }

      public MainForm()
      {
         InitializeComponent();
      }

      private void btnExecute_Click(object sender, EventArgs e)
      {
         // Start a new "task" to process the files. 
         Task.Factory.StartNew(ProcessIntData);
      }

      private void ProcessIntData()
      {
         // Get a very large array of integers. 
         int[] source = Enumerable.Range(1, 10000000).ToArray();

         // Find the numbers where num % 3 == 0 is true, returned
         // in descending order. 
         try
         {
            int[] modThreeIsZero = (from num in source.AsParallel().WithCancellation(_cancelToken.Token)
                                    where num % 3 == 0
                                    orderby num descending
                                    select num).ToArray();
            MessageBox.Show(string.Format("Found {0} numbers that match query!", modThreeIsZero.Count()));
         }
         catch (OperationCanceledException ex)
         {
            Invoke((Action)delegate
            {
               Text = ex.Message;
            });
         }
      }
   }
}
