using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RecursiveTplDataflowLinking
{
   public class DirectoryWalker
   {
      private long _directoriesRemaining;
      private readonly TransformManyBlock<string, string> _directoryBrowseBlock;
      private readonly ActionBlock<string> _fileActionBlock;

      public DirectoryWalker(Action<string> fileAction)
      {
         _directoryBrowseBlock =
            new TransformManyBlock<string, string>((Func<string, IEnumerable<string>>) GetFilesInDirectory);
         _fileActionBlock = new ActionBlock<string>(fileAction);
         _directoryBrowseBlock.LinkTo(_directoryBrowseBlock, Directory.Exists);
         _directoryBrowseBlock.LinkTo(_fileActionBlock, new DataflowLinkOptions {PropagateCompletion = true});
      }

      public Task WalkAsync(string path)
      {
         _directoriesRemaining = 1;
         _directoryBrowseBlock.Post(path);
         return _fileActionBlock.Completion;
      }

      private IEnumerable<string> GetFilesInDirectory(string path)
      {
         var dirInfo = new DirectoryInfo(path);
         var subDirectories = dirInfo.GetDirectories().Select(info => info.FullName).ToArray();
         _directoriesRemaining += subDirectories.Length;
         if (--_directoriesRemaining == 0)
            _directoryBrowseBlock.Complete();

         return dirInfo.GetFiles().Select(fileInfo => fileInfo.FullName).Concat(subDirectories);
      }
   }
}