/**
 * Клиент для именованного канала на базе APM
 */

using System;

namespace ApmPipeClient
{
   static class Program
   {
      static void Main()
      {
         // Делаем 100 клиентских запросов серверу
         for (int i = 0; i < 100; i++)
         {
            var pipeClient = new PipeClient("localhost");
            pipeClient.Send(string.Format("Request #{0}", i));
         }

         // Так как все запросы выполняются асинхронно, конструктор, скорее всего,
         // вернет управление до их завершения. Нижняя строка не дает приложению
         // завершить работу, пока не выведены все ответы
         Console.ReadLine();
      }
   }
}
