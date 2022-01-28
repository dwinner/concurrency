using System;
using System.Net;
using System.Windows.Forms;

namespace CanonicalEap
{
   public partial class MainForm : Form
   {
      private const string LocalHost = "http://localhost";

      public MainForm()
      {
         InitializeComponent();
      }

      protected override void OnClick(EventArgs e)
      {
         // Класс System.Net.WebClient поддерживает шаблон EAP
         var webClient = new WebClient();

         // В момент завершения загрузки объект WebClient генерирует
         // событие DownloadStringCompleted, вызывающее метод webClient_DownloadStringCompleted
         webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;

         // Начало асинхронной операции (например, вызова метода BeginXxx)
         webClient.DownloadStringAsync(new Uri(LocalHost));
         base.OnClick(e);
      }

      protected void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
      {
         // Этот метод гарантированно вызывается GUI-потоком
         // В случае ошибки выводим ее: в противном случае выводим загруженную строку
         MessageBox.Show(e.Error != null ? e.Error.Message : e.Result);
      }
   }
}
