using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Wrox.ProCSharp.Threading
{
   public partial class AsyncComponent : Component
   {
      private readonly Dictionary<object, AsyncOperation> _userStateDictionary =
          new Dictionary<object, AsyncOperation>();

      private SendOrPostCallback _onCompletedDelegate;

      public event EventHandler<LongTaskCompletedEventArgs> LongTaskCompleted;

      public AsyncComponent()
      {
         InitializeComponent();
         InitializeDelegates();
      }

      public AsyncComponent(IContainer container)
      {
         container.Add(this);

         InitializeComponent();
         InitializeDelegates();
      }

      private void InitializeDelegates()
      {
         _onCompletedDelegate = LongTaskCompletion;
      }

      public string LongTask(string input)
      {
         Console.WriteLine("LongTask started");
         Thread.Sleep(5000);
         Console.WriteLine("LongTask finished");
         return input.ToUpper();
      }

      public void LongTaskAsync(string input, object taskId)
      {
         AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);

         lock (_userStateDictionary)
         {
            if (_userStateDictionary.ContainsKey(taskId))
            {
               throw new ArgumentException("taskId must be unique", "taskId");
            }
            _userStateDictionary[taskId] = asyncOp;
         }

         Action<string, AsyncOperation> longTaskDelegate = DoLongTask;
         longTaskDelegate.BeginInvoke(input, asyncOp, null, null);
      }

      private void DoLongTask(string input, AsyncOperation asyncOp)
      {
         Exception e = null;
         string output = null;
         try
         {
            output = LongTask(input);
         }
         catch (Exception ex)
         {
            e = ex;
         }
         CompletionMethod(output, e, false, asyncOp);
      }

      private void CompletionMethod(string output, Exception ex, bool cancelled, AsyncOperation asyncOp)
      {
         lock (_userStateDictionary)
         {
            _userStateDictionary.Remove(asyncOp.UserSuppliedState);
         }

         // Результат операции
         asyncOp.PostOperationCompleted(_onCompletedDelegate,
            new LongTaskCompletedEventArgs(output, ex, cancelled, asyncOp.UserSuppliedState));
      }

      private void LongTaskCompletion(object operationState)
      {
         var e = operationState as LongTaskCompletedEventArgs;
         OnLongTaskCompleted(e);
      }

      protected void OnLongTaskCompleted(LongTaskCompletedEventArgs e)
      {
         if (LongTaskCompleted != null)
         {
            LongTaskCompleted(this, e);
         }
      }
   }
}
