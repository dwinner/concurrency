/**
 * Ограничение количества потоков выполнения,
 * обращающихся к ресурсу
 */

using System;
using System.Windows.Forms;

namespace SemaphoreDemo
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainForm());
      }
   }
}
