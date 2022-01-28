/**
 * Потокобезопасный словарь
 */

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentDictionarySample
{
   internal static class Program
   {
      private static void Main()
      {
         var account = new BankAccount();
         var sharedDictionary = new ConcurrentDictionary<object, int>();

         var tasks = new Task<int>[10];
         for (int i = 0; i < tasks.Length; i++)
         {
            sharedDictionary.TryAdd(i, account.Balance);
            tasks[i] = new Task<int>(keyObj =>
            {
               for (int j = 0; j < 1000; j++)
               {
                  int currentValue;
                  sharedDictionary.TryGetValue(keyObj, out currentValue);
                  sharedDictionary.TryUpdate(keyObj, currentValue + 1, currentValue);
               }

               int result;
               if (sharedDictionary.TryGetValue(keyObj, out result))
               {
                  return result;
               }

               throw new Exception(string.Format("No data item available for key {0}", keyObj));
            }, i);

            tasks[i].Start();
         }

         foreach (Task<int> aTask in tasks)
         {
            account.Balance += aTask.Result;
         }

         Console.WriteLine("Expected value {0}, Balance: {1}", 10000, account.Balance);
         Console.WriteLine("Press enter to finish");
         Console.ReadLine();
      }
   }

   internal class BankAccount
   {
      public int Balance { get; set; }
   }
}