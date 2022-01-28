using System.Threading.Tasks.Dataflow;

namespace AtomicCafe
{
   public class Restaurant
   {
      public Restaurant(int numberOfForkAndKnifePairs)
      {
         Customers = new BufferBlock<Customer>();
         ReadyToGo = new JoinBlock<Fork, Knife, Food>(new GroupingDataflowBlockOptions {Greedy = false});
         Forks.LinkTo(ReadyToGo.Target1);
         Knifes.LinkTo(ReadyToGo.Target2);
         Food.LinkTo(ReadyToGo.Target3);

         for (var i = 0; i < numberOfForkAndKnifePairs; i++)
         {
            Forks.Post(new Fork());
            Knifes.Post(new Knife());
         }
      }

      public BufferBlock<Fork> Forks { get; } = new BufferBlock<Fork>();

      public BufferBlock<Knife> Knifes { get; } = new BufferBlock<Knife>();

      public BufferBlock<Food> Food { get; } = new BufferBlock<Food>();

      public BufferBlock<Customer> Customers { get; }

      public JoinBlock<Fork, Knife, Food> ReadyToGo { get; }
   }
}