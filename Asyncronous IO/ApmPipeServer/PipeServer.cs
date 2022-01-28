using System;
using System.IO.Pipes;
using System.Text;

namespace ApmPipeServer
{
   public sealed class PipeServer
   {
      // Каждый серверный объект выполняет в канале асинхронные операции
      private readonly NamedPipeServerStream _pipeServerStream =
         new NamedPipeServerStream("Echo", PipeDirection.InOut, -1, PipeTransmissionMode.Message,
            PipeOptions.Asynchronous | PipeOptions.WriteThrough);

      public void InitConnection()
      {
         // Асинхронное принятие соединения с клиентом
         _pipeServerStream.BeginWaitForConnection(ClientConnected, null);
      }

      private void ClientConnected(IAsyncResult result)
      {
         // Клиент присоединен, принимаем другого клиента         
         var newPipeServer = new PipeServer();
         newPipeServer.InitConnection();

         // Принятие соединения с клиентом
         _pipeServerStream.EndWaitForConnection(result);

         // Асинхронное чтение запроса со стороны клиента
         var data = new byte[1000];
         _pipeServerStream.BeginRead(data, 0, data.Length, GotRequest, data);
      }

      private void GotRequest(IAsyncResult result)
      {
         // Обработка присланного клиентом запроса
         int bytesRead = _pipeServerStream.EndRead(result);
         var data = (byte[])result.AsyncState;

         // Мой сервер просто меняет регистр символов,
         // но вы можете вставить сюда любую вычислительную операцию
         data = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(data, 0, bytesRead).ToUpper().ToCharArray());

         // Асинхронная отправка ответа клиенту
         _pipeServerStream.BeginWrite(data, 0, data.Length, WriteDone, null);
      }

      private void WriteDone(IAsyncResult result)
      {
         // Ответ клиенту отправлен, закрываем соединение со своей стороны
         _pipeServerStream.EndWrite(result);
         _pipeServerStream.Close();
      }
   }
}