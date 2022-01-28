/**
 * Использование нескольких блокирующих коллекций для поставщика/потребителя
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace _19_MultiplyProducerConsumer
{
   internal static class Program
   {
      private static void Main()
      {
         // Создаем 2 блокирующие коллекции для передачи строк
         var bc1 = new BlockingCollection<string>();
         var bc2 = new BlockingCollection<string>();

         // Создаем другую блокирующую коллекцию, которая будет использоваться для передачи целых чисел
         var bc3 = new BlockingCollection<string>();

         // Создаем два массива блокирующих коллекций
         BlockingCollection<string>[] bcland2 = { bc1, bc2 };
         BlockingCollection<string>[] bcAll = { bc1, bc2, bc3 };

         var tokenSource = new CancellationTokenSource();

         // Создаем первый набор производителей
         for (int i = 0; i < 5; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (!tokenSource.IsCancellationRequested)
               {
                  string message = string.Format("Message from task {0}", Task.CurrentId);
                  // Добавляем сообщение в одну из двух доступных коллекций
                  BlockingCollection<string>.AddToAny(bcland2, message, tokenSource.Token);
                  tokenSource.Token.WaitHandle.WaitOne(1000);
               }
            }, tokenSource.Token);
         }

         // Создаем второй набор производителей
         for (int i = 0; i < 3; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (!tokenSource.IsCancellationRequested)
               {
                  string warning = string.Format("Warning from task {0}", Task.CurrentId);
                  bc3.Add(warning, tokenSource.Token);
                  tokenSource.Token.WaitHandle.WaitOne(500);
               }
            }, tokenSource.Token);
         }

         // Создаем потребителей
         for (int i = 0; i < 2; i++)
         {
            Task.Factory.StartNew(() =>
            {
               while (!tokenSource.IsCancellationRequested)
               {
                  string item;
                  int bcid = BlockingCollection<string>.TakeFromAny(bcAll, out item, tokenSource.Token);
                  Console.WriteLine("From collection {0}: {1}", bcid, item);
               }
            }, tokenSource.Token);
         }

         Console.WriteLine("Press enter to cancel tasks");
         Console.ReadLine();

         tokenSource.Cancel();

         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }
}