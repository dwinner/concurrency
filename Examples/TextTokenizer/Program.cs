/**
 * Асинхронная модель программирования
 */

using System;
using System.Windows.Forms;

namespace TextTokenizer
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new Mainform());
      }
   }
}
