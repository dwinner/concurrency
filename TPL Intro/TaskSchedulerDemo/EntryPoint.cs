using System;
using System.Windows.Forms;

namespace _11_TaskSchedulerDemo
{
   public class EntryPoint
   {
      [STAThread]
      public static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new EntryForm());
      }
   }
}