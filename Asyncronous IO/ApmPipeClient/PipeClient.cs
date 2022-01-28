using System;
using System.IO.Pipes;
using System.Text;

namespace ApmPipeClient
{
   public sealed class PipeClient
   {
      private const int DefaultByteBufferLength = 1000;
      // В канале каждый клиент выполняет асинхронную операцию
      private readonly NamedPipeClientStream _namedPipeClient;

      public PipeClient(string serverName)
      {
         _namedPipeClient = new NamedPipeClientStream(serverName, "Echo", PipeDirection.InOut,
            PipeOptions.Asynchronous | PipeOptions.WriteThrough);
         _namedPipeClient.Connect();   // Требуется вызвать Connect перед установкой ReadMode
         _namedPipeClient.ReadMode = PipeTransmissionMode.Message;
      }

      public void Send(string message)
      {
         // Асинхронная отправка данных на сервер
         byte[] output = Encoding.UTF8.GetBytes(message);
         _namedPipeClient.BeginWrite(output, 0, output.Length, WriteDone, null);
      }

      private void WriteDone(IAsyncResult result)
      {
         // Данные отправлены на сервер
         _namedPipeClient.EndWrite(result);

         // Асинхронное чтение ответа сервера
         var data = new byte[DefaultByteBufferLength];
         _namedPipeClient.BeginRead(data, 0, data.Length, GotResponse, data);
      }

      private void GotResponse(IAsyncResult result)
      {
         // Вывод ответа сервера и закрытие соединения
         int bytesRead = _namedPipeClient.EndRead(result);
         var data = result.AsyncState as byte[];
         if (data != null)
         {
            Console.WriteLine("Server response: {0}", Encoding.UTF8.GetString(data, 0, bytesRead));
         }
         _namedPipeClient.Close();
      }
   }
}