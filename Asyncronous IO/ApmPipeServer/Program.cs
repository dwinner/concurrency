/**
 * Сервер для именованного канала, реализованный при помощи APM
 */

using System;

namespace ApmPipeServer
{
   static class Program
   {
      static void Main()
      {
         // Запуск одного сервера на каждый процессор
         for (int i = 0; i < Environment.ProcessorCount; i++)
         {
            var pipeServer = new PipeServer();
            pipeServer.InitConnection();
         }

         Console.WriteLine("Press <Enter> to terminate this server application");
         Console.ReadLine();
      }
   }
}
