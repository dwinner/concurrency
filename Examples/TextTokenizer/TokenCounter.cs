using System;
using System.Collections.Generic;

namespace TextTokenizer
{
   struct WordCount
   {
      public string Word { get; set; }
      public int Count { get; set; }
   }

   class TokenCounter
   {
      private delegate void CountDelegate();

      private readonly string _data;

      private readonly Dictionary<string, int> _tokens =
         new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

      private readonly List<WordCount> _wordCounts = new List<WordCount>();

      public IList<WordCount> WordCounts { get { return _wordCounts; } }

      public TokenCounter(string data)
      {
         _data = data;
      }

      public void Count()
      {
         // Ради простоты возьмем стандартные разделители слов
         char[] splitters = { ' ', '.', ',', ';', ':', '-', '?', '!', '\t', '\n', '\r', '(', ')', '[', ']', '{', '}' };
         string[] words = _data.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
         foreach (string word in words)
         {
            int count;
            _tokens[word] = !_tokens.TryGetValue(word, out count) ? 1 : count + 1;
         }

         foreach (KeyValuePair<string, int> pair in _tokens)
         {
            _wordCounts.Add(new WordCount
            {
               Word = pair.Key,
               Count = pair.Value
            });
         }
         _wordCounts.Sort((a, b) => -a.Count.CompareTo(b.Count));
      }

      public IAsyncResult BeginCount(AsyncCallback callback, object state)
      {
         CountDelegate countDelegate = Count;
         return countDelegate.BeginInvoke(callback, state);
      }

      public void EndCount(IAsyncResult result)
      {
         // Подождать, пока операция закончится
         result.AsyncWaitHandle.WaitOne();
      }
   }
}
