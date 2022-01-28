using System;
using System.ServiceModel;
using System.Threading.Tasks.Dataflow;

namespace RemoteDispatcherViaJoiningBlocks
{
   public class GridDispatcher<T>
   {
      private readonly BufferBlock<Uri> _nodes = new BufferBlock<Uri>();

      private readonly JoinBlock<Uri, T> _scheduler =
         new JoinBlock<Uri, T>(new GroupingDataflowBlockOptions {Greedy = false});

      private readonly BufferBlock<T> _workItems = new BufferBlock<T>();

      public GridDispatcher(IGridNode<T> localService)
      {
         var localDispacther = new ActionBlock<T>(wi =>
         {
            Console.WriteLine("Executing locally");
            return localService.InvokeAsync(wi);
         }, new ExecutionDataflowBlockOptions {BoundedCapacity = 2, MaxDegreeOfParallelism = 2});

         var dispatcher = new ActionBlock<Tuple<Uri, T>>((Action<Tuple<Uri, T>>) Dispatch);

         _workItems.LinkTo(localDispacther);
         _workItems.LinkTo(_scheduler.Target2);
         _nodes.LinkTo(_scheduler.Target1);

         _scheduler.LinkTo(dispatcher);
      }

      public void RegisterNode(Uri uri) => _nodes.Post(uri);

      public void SubmitWork(T workItem) => _workItems.Post(workItem);

      private void Dispatch(Tuple<Uri, T> obj)
      {
         var channelFactory = new ChannelFactory<IGridNode<T>>(new NetTcpBinding(), new EndpointAddress(obj.Item1));
         var proxy = channelFactory.CreateChannel();
         proxy.InvokeAsync(obj.Item2)
            .ContinueWith(task =>
            {
               // ReSharper disable SuspiciousTypeConversion.Global
               ((IClientChannel) proxy).Close();
               // ReSharper restore SuspiciousTypeConversion.Global
               _nodes.Post(obj.Item1);
            });
      }
   }
}