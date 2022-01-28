using System;
using System.ServiceModel;

namespace RemoteDispatcherViaJoiningBlocks
{
   internal static class Program
   {
      private static void Main()
      {
         var host = new ServiceHost(typeof(NodeWorker));
         host.AddServiceEndpoint(typeof(IGridNode<WorkItem>), new NetTcpBinding(), "net.tcp://localhost:9000/Worker");
         host.Open();

         var grid = new GridDispatcher<WorkItem>(new NodeWorker());

         grid.RegisterNode(new Uri("net.tcp://localhost:9000/Worker"));
         grid.RegisterNode(new Uri("net.tcp://localhost:9000/Worker"));

         for (var i = 0; i < 10; i++)
            grid.SubmitWork(new WorkItem {Lhs = i, Rhs = 4});

         Console.ReadLine();
      }
   }
}