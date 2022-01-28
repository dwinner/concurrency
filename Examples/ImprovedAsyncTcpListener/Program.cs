/**
 * Улучшенный сервер обработки входящих запросов
 * Test: Microsoft Telnet > open 127.0.0.1 1234
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _20_ImprovedAsyncTcpListener
{
   class EntryPoint
   {
      private const int ConnectQueueLength = 4;
      private const int ListenPort = 1234;
      private const int MaxConnectionHandlers = 4;

      private static void HandleConnection(IAsyncResult ar)
      {
         var listener = (Socket)ar.AsyncState;
         Socket newConnection = listener.EndAccept(ar);
         byte[] msg = Encoding.UTF8.GetBytes("Hello world");
         newConnection.BeginSend(
            msg, 0, msg.Length, SocketFlags.None, CloseConnection, newConnection);
         // Поместить другой запрос в очередь
         listener.BeginAccept(HandleConnection, listener);
      }

      static void CloseConnection(IAsyncResult ar)
      {
         var theSocket = (Socket)ar.AsyncState;
         theSocket.Close();
      }

      static void Main()
      {
         var listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         listenSock.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
         listenSock.Listen(ConnectQueueLength);

         // Ожидать дескрипторов соединений
         for (int i = 0; i < MaxConnectionHandlers; i++)
         {
            listenSock.BeginAccept(HandleConnection, listenSock);
         }
         Console.WriteLine("Нажмите <Enter> для завершения");
         Console.ReadLine();
      }
   }
}
