using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpeculativeSelection
{
   public static class SpeculativeSelection
   {
      public static void Compute<TInput, TOutput>(TInput value, Action<long, TOutput> callback,
         params Func<TInput, TOutput>[] functions)
      {
         // определить счетчик для указания производимых результатов
         int resultCounter = 0;

         // Запустим задачу, которая запускает параллельный цикл,
         // в противном случае метод будет заблокирован до тех пор,
         // пока результат не будет получен и все запущенные функции 
         // не завершаться, даже если они и были завершены неудачно
         Task.Factory.StartNew(() =>
         {
            // Организуем параллельный цикл
            Parallel.ForEach(functions, (func, loopState, iterationIndex) =>
            {
               // Вычислим результат
               TOutput localResult = func(value);

               // Увеличим значение счетчика
               if (Interlocked.Increment(ref resultCounter) == 1)
               {
                  // Получили первый результат. Останавливаем цикл
                  loopState.Stop();
                  callback(iterationIndex, localResult);
               }
            });
         });
      }
   }
}