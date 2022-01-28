using FunWithCSharpAsync.Properties;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunWithCSharpAsync
{
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();
      }

      private async void btnCallMethod_Click(object sender, EventArgs e)
      {
         Text = await DoWorkAsync();
      }

      private static async Task<string> DoWorkAsync()
      {
         return await Task.Run(() =>
         {
            Thread.Sleep(10000);
            return "Done with work!";
         });
      }

      private async void btnVoidMethodCall_Click(object sender, EventArgs e)
      {
         await MethodReturningVoidAsync();
         MessageBox.Show(Resources.DoneRes);
      }

      private static async Task MethodReturningVoidAsync()
      {
         await Task.Run(() => Thread.Sleep(4000));
      }

      private async void btnMutliAwaits_Click(object sender, EventArgs e)
      {
         await Task.Run(() => Thread.Sleep(2000));
         MessageBox.Show(Resources.DoneWithFirstTaskRes);

         await Task.Run(() => Thread.Sleep(2000));
         MessageBox.Show(Resources.DoneWithSecondTaskRes);

         await Task.Run(() => Thread.Sleep(2000));
         MessageBox.Show(Resources.DoneWithThirdTaskRes);
      }
   }
}
