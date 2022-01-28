/**
 * Инкапсуляция задачи в классе Task
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _22_Task
{
   class EntryPoint
   {
      private const int ConnectQueueLength = 4;
      private const int ListenPort = 1234;

      static void Main()
      {
         var doneEvent = new ManualResetEventSlim();

         // Создание задачи для прослушивания сокета.
         var listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         try
         {
            Task listenTask = null;
            listenTask = Task.Factory.StartNew(() =>
               {
                  listenSock.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
                  listenSock.Listen(ConnectQueueLength);
                  Socket connection = listenSock.Accept();
                  listenTask.ContinueWith((previousTask) =>
                     {
                        byte[] msg = Encoding.UTF8.GetBytes("Hello world");
                        connection.Send(msg, SocketFlags.None);
                        connection.Close();
                        doneEvent.Set();
                     }
                  );
               }
            );
            Console.WriteLine("Ожидание завершения задачи...");
            doneEvent.Wait();
         }
         catch (AggregateException aggregateEx)
         {
            Console.WriteLine(aggregateEx);
         }
         listenSock.Close();
      }
   }
}
