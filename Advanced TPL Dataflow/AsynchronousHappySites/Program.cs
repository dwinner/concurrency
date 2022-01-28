using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AsynchronousHappySites
{
   internal static class Program
   {
      private static void Main()
      {
         string[] urls =
         {
            "http://www.bbc.co.uk",
            "http://www.cia.gov",
            "http://www.theregister.co.uk"
         };

         async Task<Tuple<string, bool>> IsHappyAsync(string url)
         {
            using (var client = new WebClient())
            {
               var content = await client.DownloadStringTaskAsync(url);
               return Tuple.Create(url, content.ToLower().Contains("happy"));
            }
         }

         var happySites = new List<string>();
         var isHappySiteBlock =
            new TransformBlock<string, Tuple<string, bool>>((Func<string, Task<Tuple<string, bool>>>) IsHappyAsync,
               new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 1});

         var addToHappySitesBlock = new ActionBlock<Tuple<string, bool>>(
            tuple => happySites.Add(tuple.Item1));

         isHappySiteBlock.LinkTo(addToHappySitesBlock,
            new DataflowLinkOptions {PropagateCompletion = true},
            tuple => tuple.Item2);

         isHappySiteBlock.LinkTo(DataflowBlock.NullTarget<Tuple<string, bool>>());

         foreach (var url in urls)
            isHappySiteBlock.Post(url);

         isHappySiteBlock.Complete();

         addToHappySitesBlock.Completion.Wait();
         happySites.ForEach(Console.WriteLine);
      }      
   }
}