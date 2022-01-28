/**
 * Преобразование модели асинхронного программирования на базе
 * событий в объект Task
 */

using System;
using System.Windows.Forms;

namespace EapToTask
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
         Application.Run(new MyForm());
      }
   }
}
