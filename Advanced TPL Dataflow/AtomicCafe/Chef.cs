using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AtomicCafe
{
   public class Chef
   {
      private const int DefaultFoodItemCount = 4;
      private readonly Restaurant _restaurant;

      public Chef(Restaurant restaurant) => _restaurant = restaurant;

      public async Task MakeFoodAsync()
      {
         Console.WriteLine("Chef making food");
         var knife = await _restaurant.Knifes.ReceiveAsync().ConfigureAwait(false);
         for (var nFoodItem = 0; nFoodItem < DefaultFoodItemCount; nFoodItem++)
            _restaurant.Food.Post(new Food());

         Console.WriteLine("Chef made food...");
         _restaurant.Knifes.Post(knife);
      }
   }
}