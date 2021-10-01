using System.Collections.Generic;
using System.Transactions;
using SHBank.Entity;

namespace SHBank.Model
{
    public interface ITransactionHistoryModel
    {
        TransactionHistory Save(TransactionHistory transactionHistory);
        List<TransactionHistory> FindByNameAccount (string name);
    }
}