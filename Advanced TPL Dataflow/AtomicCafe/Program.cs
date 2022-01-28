using System;
using System.Threading.Tasks.Dataflow;

namespace AtomicCafe
{
   internal static class Program
   {
      private static void Main()
      {
         var restaurant = new Restaurant(2);
         // ReSharper disable UnusedVariable
         var waiter = new Waiter(restaurant);
         // ReSharper restore UnusedVariable
         ConsoleKey key;
         var chef = new Chef(restaurant);
         var id = 1;
         while ((key = Console.ReadKey(true).Key) != ConsoleKey.Q)
            switch (key)
            {
               case ConsoleKey.C:
                  chef.MakeFoodAsync().Wait();
                  break;
               case ConsoleKey.N:
                  restaurant.Customers.Post(new Customer(id++));
                  break;
            }
      }
   }
}