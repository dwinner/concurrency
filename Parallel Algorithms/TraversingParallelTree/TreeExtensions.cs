using System;
using System.Threading.Tasks;

namespace TraversingParallelTree
{
   public static class TreeExtensions
   {
      public static void TraverseTree<T>(this Tree<T> tree, Action<T> action)
      {
         if (tree != null)
         {
            // Вызовем действие для данных
            action(tree.Data);

            // Запускаем задачи для обработки левых и правых узлов, если они есть
            if (tree.LeftNode != null && tree.RightNode != null)
            {
               Task.WaitAll(
                  Task.Factory.StartNew(() => TraverseTree(tree.RightNode, action)),
                  Task.Factory.StartNew(() => TraverseTree(tree.LeftNode, action)));
            }
         }
      }
   }
}