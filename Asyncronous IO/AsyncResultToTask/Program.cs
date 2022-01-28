/**
 * Преобразование объекта IAsyncResult в объект Task
 */

using System;
using System.Net;
using System.Threading.Tasks;

namespace AsyncResultToTask
{
   static class Program
   {
      private const string DefaultHost = "http://localhost";

      static void Main()
      {
         WebRequest webRequest = WebRequest.Create(DefaultHost);
         Task.Factory.FromAsync<WebResponse>(
            webRequest.BeginGetResponse,
            webRequest.EndGetResponse,
            null,
            TaskCreationOptions.None).ContinueWith(task =>
            {
               WebResponse webResponse = null;
               try
               {
                  webResponse = task.Result;
                  Console.WriteLine("Content length: {0}", webResponse.ContentLength);
               }
               catch (AggregateException aggrEx)
               {
                  if (aggrEx.GetBaseException() is WebException)
                     Console.WriteLine("Failed: {0}", aggrEx.GetBaseException().Message);
                  else
                     throw;
               }
               finally
               {
                  if (webResponse != null)
                     webResponse.Close();
               }
            });

         Console.ReadLine();
      }
   }
}
