/**
 * Контекст синхронизации Windows Forms
 */

using System;
using System.Windows.Forms;

namespace SyncContextWindowsForms
{
   static class Program
   {
      /// <summary>
      /// Главная точка входа для приложения.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new SyncForm());
      }
   }
}
