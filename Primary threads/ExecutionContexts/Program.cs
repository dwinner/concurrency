/**
 * Контексты исполнения
 */

using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace _04_ExecutionContexts
{
   public static class Program
   {
      static void Main()
      {
         // Помещаем данные в контекст логического вызова потока метода Main
         CallContext.LogicalSetData("Name", "Denny");

         // Заставляем поток из пула работать
         // Поток из пула имеет доступ к данным контекта логического вызова
         ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));

         // Запрещаем копирование контекста исполнения потока метода Main
         AsyncFlowControl asyncFlowControl = ExecutionContext.SuppressFlow();

         // Заставляем поток из пула работать
         // Поток из пула теперь НЕ имеет доступа к данным контекста логического вызова
         ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));

         // Восстанавливаем копирование контекста исполнения потока метода Main
         // на случай будущей работы с другими потоками из пула
         asyncFlowControl.Undo();   // Или так: ExecutionContext.RestoreFlow();

         ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));

         Console.ReadKey();
      }
   }
}
