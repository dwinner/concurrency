/**
 * Параллельный поиск в дереве
 */

using System;

namespace ParallelTreeSearchSample
{
   static class Program
   {
      static void Main()
      {
         // Создадим и заполним простое дерево
         Tree<int> tree = PopulateTree(new Tree<int>(), new Random(2));

         // Ищем значение 183 в дереве
         int result = tree.ParallelSearch(item =>
         {
            if (item == 183)
            {
               Console.WriteLine("Value: {0}", item);
            }
            return item == 183;
         });

         Console.WriteLine("Search match ? {0}", result);
         Console.Write("Press enter to finish");
         Console.ReadLine();
      }

      private static Tree<int> PopulateTree(Tree<int> parentNode, Random random, int depth = 0)
      {
         parentNode.Data = random.Next(1, 1000);
         if (depth < 10)
         {
            parentNode.LeftNode = new Tree<int>();
            parentNode.RightNode = new Tree<int>();
            PopulateTree(parentNode.LeftNode, random, depth + 1);
            PopulateTree(parentNode.RightNode, random, depth + 1);
         }

         return parentNode;
      }
   }
}
