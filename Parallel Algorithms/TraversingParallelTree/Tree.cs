namespace TraversingParallelTree
{
   public class Tree<T>
   {
      public Tree<T> LeftNode { get; set; }

      public Tree<T> RightNode { get; set; }

      public T Data { get; set; }
   }
}