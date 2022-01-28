using System.Net;
using System.Windows.Forms;
using SyncContext.Library;
using SyncContextWindowsForms.Properties;

namespace SyncContextWindowsForms
{
   public sealed partial class SyncForm : Form
   {
      private const int DefaultWindowHeight = 100;
      private const int DefaultWindowWidth = 400;
      private const string DefaultHost = "http://localhost:1315/";

      public SyncForm()
      {
         InitializeComponent();
         Text = Resources.StartTitleResource;
         Width = DefaultWindowWidth;
         Height = DefaultWindowHeight;
      }

      protected override void OnMouseClick(MouseEventArgs e)
      {
         // GUI-поток инициирует асинхронный веб-запрос
         Text = Resources.StartWebRequestMsg;
         WebRequest webRequest = WebRequest.Create(DefaultHost);
         webRequest.BeginGetResponse(SyncContextUtils.SyncContextCallback(asyncResult =>
         {
            // Если мы попали сюда, это должен быть GUI-поток,
            // значит, обновляем пользовательский интерфейс
            var request = asyncResult.AsyncState as WebRequest;
            if (request != null)
            {
               try
               {
                  using (var webResponse = request.EndGetResponse(asyncResult))
                  {
                     Text = string.Format(Resources.ResponseLengthMsg, webResponse.ContentLength);
                  }
               }
               catch (WebException webEx)
               {
                  Text = webEx.Message;
               }
            }
         }), webRequest);
         base.OnMouseClick(e);
      }
   }
}
