using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace IfElseSwitchLinking
{
   // ReSharper disable UnusedMember.Global
   public class CsvImporter
      // ReSharper restore UnusedMember.Global
   {
      public CsvImporter()
      {
         var databaseQueryBlock =
            new TransformManyBlock<string, object[]>((Func<string, IEnumerable<object[]>>) ExecuteQuery);
         var rowToGoLedgerBlock = new TransformManyBlock<object[], ILedgerEntry>(
            (Func<object[], IEnumerable<ILedgerEntry>>) MapDatabaseRowToObject);
         var debitBlock = new ActionBlock<ILedgerEntry>((Action<ILedgerEntry>) WriteDebitEntry);
         var creditBlock = new ActionBlock<ILedgerEntry>((Action<ILedgerEntry>) WriteCreditEntry);

         var unknownLedgerEntryBlock = new ActionBlock<ILedgerEntry>((Action<ILedgerEntry>) LogUnknownLedgerEntryType);

         databaseQueryBlock.LinkTo(rowToGoLedgerBlock);
         rowToGoLedgerBlock.LinkTo(debitBlock, entry => entry.IsDebit); // If is debit
         rowToGoLedgerBlock.LinkTo(creditBlock, entry => entry.IsCredit); // If is credit
         rowToGoLedgerBlock.LinkTo(unknownLedgerEntryBlock); // Other cases
      }

      //public void Export(string connectionString) => _databaseQueryBlock.Post(connectionString);

      private void LogUnknownLedgerEntryType(ILedgerEntry obj)
      {
      }

      private void WriteCreditEntry(ILedgerEntry obj)
      {
      }

      private void WriteDebitEntry(ILedgerEntry obj)
      {
      }

      private IEnumerable<ILedgerEntry> MapDatabaseRowToObject(object[] arg)
      {
         yield break;
      }

      private IEnumerable<object[]> ExecuteQuery(string arg)
      {
         yield break;
      }
   }
}