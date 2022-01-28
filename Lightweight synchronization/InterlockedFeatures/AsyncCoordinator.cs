using System;
using System.Threading;

namespace InterlockedFeatures
{
   /// <summary>
   /// Легковесный координатор потоков
   /// </summary>
   internal sealed class AsyncCoordinator
   {
      private int _operationCount = 1; // Уменьшается на 1 методом AllBegun
      private int _statusReported; // 0=false, 1=true
      private Action<CoordinationStatus> _callback;
      private Timer _timer;

      /// <summary>
      /// Добавление асинхронной операции к группе таких операций
      /// </summary>
      /// <param name="operationsToAdd">Количество добавляемых операций</param>
      /// <remarks>Этот метод следует вызвать до метода BeginXxx</remarks>
      public void AboutToBegin(int operationsToAdd = 1)
      {
         Interlocked.Add(ref _operationCount, operationsToAdd);
      }

      /// <summary>
      /// Уведомляет об окончании асинхронной операции
      /// <remarks>Этот метод следует вызвать после метода EndXxx</remarks>
      /// </summary>
      public void JustEnded()
      {
         if (Interlocked.Decrement(ref _operationCount) == 0)
            ReportStatus(CoordinationStatus.AllDone);
      }

      /// <summary>
      /// Уведомляет о начале выполнения всех асинхронных операций
      /// </summary>
      /// <param name="callbackAction">Действие по завершению операций</param>
      /// <param name="timeout">Время, отведенное на выполнение</param>
      /// <summary>Этот метод следует вызвать после вызова всех методов BeginXxx</summary>
      public void AllBegun(Action<CoordinationStatus> callbackAction, int timeout = Timeout.Infinite)
      {
         _callback = callbackAction;
         if (timeout != Timeout.Infinite)
            _timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
         JustEnded();
      }

      private void TimeExpired(object state)
      {
         ReportStatus(CoordinationStatus.Timeout);
      }

      /// <summary>
      /// Уведомляет об отмене операций
      /// </summary>
      public void Cancel()
      {
         ReportStatus(CoordinationStatus.Cancel);
      }

      private void ReportStatus(CoordinationStatus allDone)
      {
         // Note: Если состояние ни разу не передавалось, передайте его, в противном случае игнорируйте его
         if (Interlocked.Exchange(ref _statusReported, 1) == 0)
            _callback(allDone);
      }
   }
}
