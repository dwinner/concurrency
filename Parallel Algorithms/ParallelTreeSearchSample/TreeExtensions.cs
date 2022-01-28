using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTreeSearchSample
{
   public static class TreeExtensions
   {
      public static T ParallelSearch<T>(this Tree<T> tree, Func<T, bool> searchFunction)
      {
         var tokenSource = new CancellationTokenSource();   // Создаем токен отмены         
         Wrapper<T> result = PerformSearch(tree, searchFunction, tokenSource);   // Ищем...
         return result == null ? default(T) : result.Value;
      }

      private static Wrapper<T> PerformSearch<T>(Tree<T> tree, Func<T, bool> searchFunction, CancellationTokenSource tokenSource)
      {
         Wrapper<T> result = null;  // Определяем результат

         if (tree != null) // Ищем только если есть что искать
         {
            if (searchFunction(tree.Data))   // Применим функцию поиска к текущему дереву
            {
               tokenSource.Cancel();   // Отменяем всех остальных               
               result = new Wrapper<T> { Value = tree.Data };  // Устанавливаем результат
            }
            else
            {
               if (tree.LeftNode != null && tree.RightNode != null)  // Мы не нашли результат - продолжим поиск
               {
                  // Запускаем задачу для левого узла
                  Task<Wrapper<T>> leftTask =
                     Task<Wrapper<T>>.Factory.StartNew(
                        () => PerformSearch(tree.RightNode, searchFunction, tokenSource), tokenSource.Token);
                  // Запускаем задачу для правого узла
                  Task<Wrapper<T>> rightTask =
                     Task<Wrapper<T>>.Factory.StartNew(
                        () => PerformSearch(tree.LeftNode, searchFunction, tokenSource), tokenSource.Token);
                  try
                  {
                     result = leftTask.Result ?? (rightTask.Result); // Установим результат из задач
                  }
                  catch (AggregateException) {/* Note: "проглатываем" */}
               }
            }
         }

         return result;
      }

      public class Wrapper<T>
      {
         public T Value;
      }
   }
}