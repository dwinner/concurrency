using System;
using System.Threading.Tasks;

namespace AtomicCafe
{
   public class Customer
   {
      private readonly int _id;
      //private JoinBlock<Fork, Knife, Food> _dinningBlock;
      //private ActionBlock<Tuple<Fork, Knife, Food>> _eatingBlock;

      public Customer(int id)
      {
         Console.WriteLine("New hungry customer {0}", _id);
         _id = id;
      }

      // ReSharper disable UnusedParameter.Global
      public async Task EatAsync(Fork fork, Knife knife, Food food)
         // ReSharper restore UnusedParameter.Global
      {
         Console.WriteLine("Yummy {0}", _id);
         await Task.Delay(TimeSpan.FromSeconds(2));
         Console.WriteLine("Burp {0}", _id);
      }
   }
}