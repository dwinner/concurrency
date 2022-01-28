namespace SyncRootIdiom
{
   public static class DemoFactory
   {
      public static IDemo NewDemo(bool isSynchronized)
      {
         IDemo demo = new Demo();
         return isSynchronized ? new SyncDemo(demo) : demo;
      }

      private sealed class Demo : IDemo
      {
         public bool IsSynchronized
         {
            get
            {
               return false;
            }
         }

         public void DoThis()
         {
         }

         public void DoThat()
         {
         }
      }

      private sealed class SyncDemo : IDemo
      {
         private readonly IDemo _sourceDemo;
         private readonly object _syncRoot = new object();

         public SyncDemo(IDemo sourceDemo)
         {
            _sourceDemo = sourceDemo;
         }

         public bool IsSynchronized
         {
            get
            {
               return true;
            }
         }

         public void DoThis()
         {
            lock (_syncRoot)
            {
               _sourceDemo.DoThis();
            }
         }

         public void DoThat()
         {
            lock (_syncRoot)
            {
               _sourceDemo.DoThat();
            }
         }
      }
   }
}