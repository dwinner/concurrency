/**
 * Обслуживание входящих запросов с применением пула потоков
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _19_AsyncTcpListener
{
   class Program
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
            Socket newConnection = listenSock.Accept();
            byte[] msg = Encoding.UTF8.GetBytes("Hello world");
            newConnection.BeginSend(msg, 0, msg.Length, SocketFlags.None, null, null);
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
