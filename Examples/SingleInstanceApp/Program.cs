/**
 * Ограничение кол-ва экземпляров приложения до одного
 */

using System;
using System.Threading;
using System.Windows.Forms;

namespace SingleInstanceApp
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         bool createdNew;

         // Note: поставьте перед именем мьютекса префикс Global\,
         // чтобы пользователи из других сеансов не смогли запустить приложение

         Mutex mutex = new Mutex(true, "CSharpHowTo_SingleInstanceApp", out createdNew);

         if (createdNew)
         {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
         }
         else
         {
            MessageBox.Show("You can only run a single instance of this app!");
         }
      }
   }
}
