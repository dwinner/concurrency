/**
 * Обслуживание входящих запросов в одном потоке
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _18_SyncTcpListener
{
   class EntryPoint
   {
      private const int ConnectQueueLength = 4;
      private const int ListenPort = 1234;

      static void ListenForRequests()
      {
         var listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         listenSock.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
         listenSock.Listen(ConnectQueueLength);
         while (true)
         {
            using (Socket newConnection = listenSock.Accept())
            {
               // Отправить данные
               byte[] msg = Encoding.UTF8.GetBytes("Hello world");
               newConnection.Send(msg, SocketFlags.None);
            }
         }
      }

      static void Main()
      {
         // Запустить прослушивающий поток
         var listener = new Thread(ListenForRequests) { IsBackground = true };
         listener.Start();
         Console.WriteLine("Нажмите <Enter> для завершения");
         Console.ReadLine();
      }
   }
}
