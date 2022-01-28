/**
 * Синхронизатор задач по фазам аглоритма
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarrierSample
{
   class Program
   {
      static void Main()
      {
         const int numberTasks = 2;
         const int partitionSize = 1000000;
         var data = new List<string>(FillData(partitionSize * numberTasks));

         var barrier = new Barrier(numberTasks + 1);

         var tasks = new Task<int[]>[numberTasks];
         for (int i = 0; i < numberTasks; i++)
         {
            int jobNumber = i;
            tasks[i] = Task.Run(() => CalculationInTask(jobNumber, partitionSize, barrier, data));
         }

         barrier.SignalAndWait();
         var resultCollection = tasks[0].Result.Zip(tasks[1].Result, (first, second) => first + second);

         char aChar = 'a';
         int sum = 0;
         foreach (var x in resultCollection)
         {
            Console.WriteLine("{0}, count: {1}", aChar++, x);
            sum += x;
         }

         Console.WriteLine("main finished {0}", sum);
         Console.WriteLine("remaining {0}, phase {1}", barrier.ParticipantsRemaining, barrier.CurrentPhaseNumber);
      }

      static int[] CalculationInTask(int jobNumber, int partitionSize, Barrier aBarrier, IEnumerable<string> coll)
      {
         var data = new List<string>(coll);
         int start = jobNumber * partitionSize;
         int end = start + partitionSize;
         Console.WriteLine("Task {0}: partition from {1} to {2}", Task.CurrentId, start, end);
         var charCount = new int[26];
         for (int j = 0; j < end; j++)
         {
            char c = data[j][0];
            charCount[c - 97]++;
         }
         Console.WriteLine("Calculation completed from task {0}. {1} times a, {2} times z", Task.CurrentId, charCount[0],
            charCount[25]);

         aBarrier.RemoveParticipant();
         Console.WriteLine("Task {0} removed from barrier, remaining participants {1}", Task.CurrentId,
            aBarrier.ParticipantsRemaining);

         return charCount;
      }

      public static IEnumerable<string> FillData(int size)
      {
         var data = new List<string>(size);
         var random = new Random();
         for (int i = 0; i < size; i++)
         {
            data.Add(GetString(random));
         }

         return data;
      }

      private static string GetString(Random random)
      {
         const int capacity = 6;
         var stringBuilder = new StringBuilder(capacity);
         for (int i = 0; i < capacity; i++)
         {
            stringBuilder.Append((char)(random.Next(26) + 97));
         }

         return stringBuilder.ToString();
      }
   }
}
