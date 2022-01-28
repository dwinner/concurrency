/**
 * Контекст синхронизации WPF
 */

using System.Net;
using SyncContext.Library;

namespace SyncContextWpf
{
   /// <summary>
   /// Логика взаимодействия для MainWindow.xaml
   /// </summary>
   public partial class MainWindow
   {
      private const string DefaultHost = "http://localhost:1315/";

      public MainWindow()
      {
         InitializeComponent();
      }

      private void StartRequestOnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         // GUI-поток инициирует асинхронный веб-запрос
         Title = Properties.Resources.WebRequestInitiatedMsg;
         WebRequest webRequest = WebRequest.Create(DefaultHost);
         webRequest.BeginGetResponse(SyncContextUtils.SyncContextCallback(result =>
         {
            // Если мы попали сюда, это должен быть GUI-поток,
            // значит, обновляем пользовательский интерфейс
            var request = result.AsyncState as WebRequest;
            if (request != null)
            {
               try
               {
                  using (WebResponse response = request.EndGetResponse(result))
                  {
                     Title = string.Format("Content length: {0}", response.ContentLength);
                  }
               }
               catch (WebException webEx)
               {
                  Title = webEx.Message;
               }
            }
         }), webRequest);
      }
   }
}
