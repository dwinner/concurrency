/**
 * Обход деревьев в параллельном режиме
 */

using System;

namespace TraversingParallelTree
{
   static class Program
   {
      static void Main()
      {
         // Создаем и заполняем простое дерево
         Tree<int> tree = PopulateTree(new Tree<int>(), new Random());

         // Обходим дерево, печатая четные значения
         tree.TraverseTree(item =>
         {
            if (item % 2 == 0)
            {
               Console.WriteLine("Item {0}", item);
            }
         });

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
