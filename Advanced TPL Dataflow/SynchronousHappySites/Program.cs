using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace SynchronousHappySites
{
   internal static class Program
   {
      private static readonly string[] _HappySites =
      {
         "http://www.bbc.co.uk",
         "http://www.cia.gov",
         "http://www.theregister.co.uk"
      };

      private static void Main()
      {
         Tuple<string, bool> IsHappy(string url)
         {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            using (var client = new WebClient())
            {
               var content = client.DownloadString(url);
               return Tuple.Create(url, content.ToLower().Contains("happy"));
            }
         }

         var happySites = new List<string>();
         var isHappySiteBlock = new TransformBlock<string, Tuple<string, bool>>(
            (Func<string, Tuple<string, bool>>) IsHappy,
            new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 1});

         var addToHappySiteBlock = new ActionBlock<Tuple<string, bool>>(tuple => happySites.Add(tuple.Item1));

         isHappySiteBlock.LinkTo(
            addToHappySiteBlock, new DataflowLinkOptions {PropagateCompletion = true}, tuple => tuple.Item2);

         isHappySiteBlock.LinkTo(
            DataflowBlock.NullTarget<Tuple<string, bool>>());

         Array.ForEach(_HappySites, url => isHappySiteBlock.Post(url));

         isHappySiteBlock.Complete();
         addToHappySiteBlock.Completion.Wait();
         happySites.ForEach(Console.WriteLine);
      }      
   }
}