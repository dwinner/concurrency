namespace InterlockedFeatures
{
   /// <summary>
   /// Статусы координатора потоков
   /// </summary>
   internal enum CoordinationStatus
   {
      /// <summary>
      /// Все операции завершены
      /// </summary>
      AllDone,

      /// <summary>
      /// Операции не успели завершиться по таймауту
      /// </summary>
      Timeout,

      /// <summary>
      /// Операции были отменены
      /// </summary>
      Cancel
   }
}